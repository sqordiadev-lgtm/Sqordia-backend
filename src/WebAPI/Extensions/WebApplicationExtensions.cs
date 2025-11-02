using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Sqordia.Persistence.Contexts;
using WebAPI.Middleware;
using System.Text.Json;

namespace WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        try
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                logger.LogWarning("No connection string configured. Skipping database setup.");
                return;
            }

            var db = services.GetRequiredService<ApplicationDbContext>();

            logger.LogInformation("Applying database migrations (will create database if it doesn't exist)...");
            
            // MigrateAsync will create the database if it doesn't exist
            await db.Database.MigrateAsync();
            
            logger.LogInformation("Database migrations completed successfully.");
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 18456 || sqlEx.Number == 4060)
        {
            logger.LogWarning(sqlEx, "Database connection failed. This may be because the database doesn't exist yet. Attempting to create database...");
            
            try
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var builder = new SqlConnectionStringBuilder(connectionString);
                    var databaseName = builder.InitialCatalog;
                    builder.InitialCatalog = "master";
                    
                    using var masterConnection = new SqlConnection(builder.ConnectionString);
                    await masterConnection.OpenAsync();
                    
                    var createDbCommand = masterConnection.CreateCommand();
                    createDbCommand.CommandText = $@"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
                        BEGIN
                            CREATE DATABASE [{databaseName}];
                        END";
                    await createDbCommand.ExecuteNonQueryAsync();
                    
                    logger.LogInformation("Database created successfully. Applying migrations...");
                    
                    var db = services.GetRequiredService<ApplicationDbContext>();
                    await db.Database.MigrateAsync();
                    
                    logger.LogInformation("Database migrations completed successfully.");
                }
            }
            catch (Exception createEx)
            {
                logger.LogError(createEx, "Failed to create database. Application will continue without database connectivity.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations. Application will continue without database connectivity.");
        }
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        var environment = app.Environment;

        // Configure Swagger for development
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sqordia API v1");
                options.RoutePrefix = "swagger";
            });
        }

        // HTTPS redirection (can be disabled in Docker if needed)
        if (!environment.IsDevelopment() || Environment.GetEnvironmentVariable("DISABLE_HTTPS_REDIRECT") != "true")
        {
            app.UseHttpsRedirection();
        }

        // Custom middleware - Error handling should be first
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<PerformanceMiddleware>();

        // Localization middleware - Must be before MVC
        app.UseRequestLocalization();

        // Rate limiting middleware
        app.UseIpRateLimiting();

        // CORS
        app.UseCors("AllowAll");

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        // Health checks
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = report.Status.ToString(),
                    timestamp = DateTime.UtcNow,
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description
                    })
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        });

        return app;
    }
}

