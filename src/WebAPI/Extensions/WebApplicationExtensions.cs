using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Npgsql;
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
        catch (SqlException sqlEx) when (sqlEx.Number == 18456 || sqlEx.Number == 4060 || sqlEx.Number == 40925)
        {
            if (sqlEx.Number == 40925)
            {
                logger.LogError(sqlEx, "Azure SQL Database connection failed. Error 40925 typically indicates authentication or network issues.");
                logger.LogWarning("Please check:");
                logger.LogWarning("1. DB_USERNAME and DB_PASSWORD environment variables are set correctly");
                logger.LogWarning("2. Azure SQL firewall allows connections from this IP address");
                logger.LogWarning("3. The database server is accessible from the container");
                logger.LogWarning("Application will continue without database connectivity. Some features may not work.");
            }
            else
            {
                logger.LogWarning(sqlEx, "SQL Server database connection failed. This may be because the database doesn't exist yet. Attempting to create database...");
                
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
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == "3D000" || pgEx.SqlState == "28P01")
        {
            // PostgreSQL error codes:
            // 3D000 = database does not exist
            // 28P01 = authentication failed
            logger.LogWarning(pgEx, "PostgreSQL database connection failed. Error: {Message}", pgEx.Message);
            logger.LogInformation("For PostgreSQL, the database should be created manually or via migrations. Railway creates databases automatically.");
        }
        catch (PostgresException pgEx)
        {
            // PostgreSQL-specific errors
            logger.LogError(pgEx, "PostgreSQL error occurred while applying database migrations. Error Code: {SqlState}, Message: {Message}", pgEx.SqlState, pgEx.Message);
            logger.LogInformation("Application will continue without database connectivity. Please check your DATABASE_URL environment variable.");
        }
        catch (Exception ex)
        {
            // Check if it's a SQL Server exception (shouldn't happen with PostgreSQL)
            if (ex.Message.Contains("SqlException") || ex.Message.Contains("Microsoft.Data.SqlClient"))
            {
                logger.LogError(ex, "SQL Server exception detected, but PostgreSQL is expected. Please verify DATABASE_URL is set correctly in Railway.");
                logger.LogInformation("DATABASE_URL should be set to: ${{ Postgres.DATABASE_URL }} (replace Postgres with your database service name)");
            }
            else
            {
                logger.LogError(ex, "An error occurred while applying database migrations. Application will continue without database connectivity.");
            }
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

