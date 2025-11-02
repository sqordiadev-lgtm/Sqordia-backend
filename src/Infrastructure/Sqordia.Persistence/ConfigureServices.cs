using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqordia.Application.Common.Interfaces;
using Sqordia.Persistence.Contexts;
using Sqordia.Persistence.Interceptors;
using Sqordia.Persistence.Repositories;

namespace Sqordia.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register interceptor first
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        // Configure database
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            // Get connection string from configuration
            var connectionString = GetConnectionString(configuration);

            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlServerOptions.CommandTimeout(60);
                });
            options.AddInterceptors(serviceProvider.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Register generic repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        // Get the connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // If no connection string, throw an error with more details
        if (string.IsNullOrEmpty(connectionString))
        {
            var availableConnectionStrings = string.Join(", ", configuration.GetSection("ConnectionStrings").GetChildren().Select(cs => cs.Key));
            throw new InvalidOperationException($"No database connection string found. Please configure 'DefaultConnection' in appsettings. Available connection strings: {availableConnectionStrings}");
        }

        return connectionString;
    }
}
