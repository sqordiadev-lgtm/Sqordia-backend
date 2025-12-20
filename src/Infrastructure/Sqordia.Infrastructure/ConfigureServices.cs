using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using SendGrid;
using Sqordia.Application.Common.Interfaces;
using Sqordia.Application.Common.Security;
using Sqordia.Application.Services;
using Sqordia.Application.Services.Implementations;
using Sqordia.Infrastructure.Services;
using Sqordia.Infrastructure.Identity;
using Sqordia.Infrastructure.Localization;
using IIdentityService = Sqordia.Application.Common.Interfaces.IIdentityService;
using IJwtTokenService = Sqordia.Application.Common.Interfaces.IJwtTokenService;

namespace Sqordia.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // HTTP Context for getting client IP address
        services.AddHttpContextAccessor();

        // Email service (SendGrid) - Required for email verification
        // Try multiple configuration sources: environment variable, then appsettings files
        var sendGridApiKey = configuration["SendGrid:ApiKey"];
        
        // If empty string from environment variable, try to get from appsettings
        if (string.IsNullOrWhiteSpace(sendGridApiKey) || sendGridApiKey == string.Empty)
        {
            sendGridApiKey = configuration.GetSection("SendGrid")["ApiKey"];
        }
        
        if (string.IsNullOrWhiteSpace(sendGridApiKey))
        {
            throw new InvalidOperationException(
                "SendGrid API key is required. Please set the SENDGRID_API_KEY environment variable or configure it in appsettings.json or appsettings.Development.json");
        }
        
        services.AddTransient<ISendGridClient>(sp =>
            new SendGridClient(sendGridApiKey));
        services.AddTransient<IEmailService>(sp =>
            new EmailService(
                sp.GetRequiredService<ISendGridClient>(),
                configuration["SendGrid:FromEmail"]!,
                configuration["SendGrid:FromName"]!,
                sp.GetRequiredService<ILogger<EmailService>>(),
                sp.GetRequiredService<ILocalizationService>()));

        // Security service - Required for password hashing
        services.AddTransient<ISecurityService, SecurityService>();

        // Identity services - Required for authentication
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<IAccountLockoutService, AccountLockoutService>();
        services.AddTransient<ITotpService, TotpService>();

        // Localization service - Required for bilingual support
        services.AddSingleton<ILocalizationService, LocalizationService>();

        // AI service - Required for business plan generation
        // Configure from both appsettings and environment variables
        // Priority: Environment variables > appsettings.json
        services.Configure<OpenAISettings>(configuration.GetSection("AI:OpenAI"));
        
        // Post-configure to allow environment variables to override appsettings
        // This runs AFTER the initial Configure, so env vars will override
        services.PostConfigure<OpenAISettings>(options =>
        {
            // Try environment variables first (highest priority)
            var envApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                          ?? Environment.GetEnvironmentVariable("OpenAI__ApiKey")
                          ?? Environment.GetEnvironmentVariable("AI__OpenAI__ApiKey");
            
            // Then try configuration (appsettings.json)
            var configApiKey = configuration["AI:OpenAI:ApiKey"]
                            ?? configuration["OpenAI:ApiKey"];
            
            // Use first non-empty value found
            var apiKey = envApiKey ?? configApiKey;
            
            // Use first non-empty value found
            if (!string.IsNullOrEmpty(apiKey) && apiKey != "TODO: Add OpenAI API key" && !apiKey.Contains("TODO"))
            {
                options.ApiKey = apiKey;
            }
            
            // Same for model
            var envModel = Environment.GetEnvironmentVariable("OPENAI_MODEL")
                        ?? Environment.GetEnvironmentVariable("OpenAI__Model")
                        ?? Environment.GetEnvironmentVariable("AI__OpenAI__Model");
            
            var configModel = configuration["AI:OpenAI:Model"]
                           ?? configuration["OpenAI:Model"];
            
            var model = envModel ?? configModel;
            
            if (!string.IsNullOrEmpty(model))
            {
                options.Model = model;
            }
        });
        
        services.AddSingleton<IAIService, OpenAIService>();

        // Document export service - Required for PDF/Word export
        services.AddTransient<IDocumentExportService, DocumentExportService>();

        // Financial projection service - Required for financial calculations and projections
        services.AddTransient<IFinancialProjectionService, FinancialProjectionService>();

        // Admin dashboard service - Required for admin management and analytics
        services.AddTransient<IAdminDashboardService, AdminDashboardService>();
        
        // Subscription service
        services.AddScoped<Sqordia.Application.Services.ISubscriptionService, SubscriptionService>();

        return services;
    }
}
