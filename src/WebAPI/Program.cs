using Sqordia.Application;
using Sqordia.Infrastructure;
using Sqordia.Persistence;
using WebAPI.Configuration;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

var customConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (!string.IsNullOrEmpty(customConnectionString))
{
    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        { "ConnectionStrings:DefaultConnection", customConnectionString }
    });
}

builder.Services.AddApplicationConfiguration(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsServices();
builder.Services.AddLocalizationServices();
builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddHealthCheckServices();

var app = builder.Build();

await app.ApplyDatabaseMigrationsAsync();
app.ConfigureMiddleware();

app.Run();

public partial class Program { }
