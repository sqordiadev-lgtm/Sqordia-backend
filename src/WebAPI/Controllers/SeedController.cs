using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sqordia.Persistence.Contexts;
using System.Data.SqlClient;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeedController> _logger;

        public SeedController(ApplicationDbContext context, ILogger<SeedController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("database")]
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");
                
                // Log the actual connection string being used
                var connectionString = _context.Database.GetConnectionString();
                _logger.LogInformation($"Using connection string: {connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0))}...");

                // Read and split the SQL script by GO statements
                var sqlScript = await System.IO.File.ReadAllTextAsync("Scripts/SeedAzureDatabase.sql");
                var statements = sqlScript.Split(new[] { "GO", "go" }, StringSplitOptions.RemoveEmptyEntries);

                // Execute each statement
                foreach (var statement in statements)
                {
                    var trimmedStatement = statement.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedStatement) && 
                        !trimmedStatement.StartsWith("--") && 
                        !trimmedStatement.StartsWith("PRINT"))
                    {
                        try
                        {
                            await _context.Database.ExecuteSqlRawAsync(trimmedStatement);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Statement execution warning: {ex.Message}");
                            // Continue with next statement
                        }
                    }
                }

                _logger.LogInformation("Database seeding completed successfully!");

                return Ok(new
                {
                    success = true,
                    message = "Database seeded successfully!",
                    adminUser = new
                    {
                        email = "admin@sqordia.com",
                        password = "Sqordia2025!"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding database");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error seeding database",
                    error = ex.Message
                });
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetSeedStatus()
        {
            try
            {
                // Check if roles exist
                var rolesCount = await _context.Roles.CountAsync();
                var permissionsCount = await _context.Permissions.CountAsync();
                var usersCount = await _context.Users.CountAsync();

                return Ok(new
                {
                    rolesCount,
                    permissionsCount,
                    usersCount,
                    isSeeded = rolesCount > 0 && permissionsCount > 0 && usersCount > 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking seed status");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error checking seed status",
                    error = ex.Message
                });
            }
        }

        [HttpGet("config")]
        public IActionResult GetConfiguration()
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                
                return Ok(new
                {
                    connectionString = connectionString?.Substring(0, Math.Min(100, connectionString?.Length ?? 0)) + "...",
                    environment,
                    fullConnectionString = connectionString
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting configuration");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error getting configuration",
                    error = ex.Message
                });
            }
        }
    }
}
