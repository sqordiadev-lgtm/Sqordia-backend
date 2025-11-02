namespace Sqordia.Application.Common.Security;

// TODO: Security service interface
public interface ISecurityService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
    string GenerateSecureToken(int length = 32);
}
