using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebAPI.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<GoogleOAuthSettings>(configuration.GetSection(GoogleOAuthSettings.SectionName));
        services.Configure<SendGridSettings>(configuration.GetSection(SendGridSettings.SectionName));
        services.PostConfigure<JwtSettings>(options =>
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (!string.IsNullOrEmpty(jwtSecret))
            {
                options.Secret = jwtSecret;
            }
        });

        services.PostConfigure<SendGridSettings>(options =>
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            if (!string.IsNullOrEmpty(apiKey))
            {
                options.ApiKey = apiKey;
            }

            var fromEmail = Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL");
            if (!string.IsNullOrEmpty(fromEmail))
            {
                options.FromEmail = fromEmail;
            }
        });

        services.PostConfigure<GoogleOAuthSettings>(options =>
        {
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_ID");
            if (!string.IsNullOrEmpty(clientId))
            {
                options.ClientId = clientId;
            }

            var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_OAUTH_CLIENT_SECRET");
            if (!string.IsNullOrEmpty(clientSecret))
            {
                options.ClientSecret = clientSecret;
            }
        });

        services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();
        services.AddSingleton<IValidateOptions<SendGridSettings>, SendGridSettingsValidator>();

        return services;
    }
}

