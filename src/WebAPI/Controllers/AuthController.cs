using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sqordia.Application.Services;
using System.Security.Claims;
using Sqordia.Contracts.Requests.Auth;

namespace WebAPI.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authenticationService.RegisterAsync(request);
        return HandleResult(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authenticationService.LoginAsync(request);
        return HandleResult(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authenticationService.RefreshTokenAsync(request);
        return HandleResult(result);
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var result = await _authenticationService.RevokeTokenAsync(request);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await _authenticationService.LogoutAsync(request);
        return HandleResult(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _authenticationService.GetCurrentUserAsync(userId);
        return HandleResult(result);
    }

    [HttpPost("send-verification-email")]
    [AllowAnonymous]
    public async Task<IActionResult> SendVerificationEmail([FromBody] SendEmailVerificationRequest request)
    {
        var result = await _authenticationService.SendEmailVerificationAsync(request);
        return HandleResult(result);
    }

    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var result = await _authenticationService.VerifyEmailAsync(request);
        return HandleResult(result);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var result = await _authenticationService.ForgotPasswordAsync(request);
        return HandleResult(result);
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authenticationService.ResetPasswordAsync(request);
        return HandleResult(result);
    }

    /// <summary>
    /// Authenticate with Google OAuth
    /// </summary>
    /// <param name="request">Google OAuth request with ID token and access token</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthenticateWithGoogle([FromBody] GoogleAuthRequest request)
    {
        // TODO: In a real implementation, you would validate the Google ID token
        // For now, we'll extract user information from the token
        // This is a simplified implementation - in production, you should validate the token with Google
        
        try
        {
            // Extract user information from Google token (simplified)
            // In production, use Google's token validation
            var googleId = "google_" + Guid.NewGuid().ToString("N")[..8]; // Simplified for demo
            var email = "user@example.com"; // Extract from token
            var firstName = "John"; // Extract from token
            var lastName = "Doe"; // Extract from token
            var profilePictureUrl = "https://via.placeholder.com/150"; // Extract from token

            var result = await _authenticationService.AuthenticateWithGoogleAsync(
                googleId, email, firstName, lastName, profilePictureUrl);
            
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Invalid Google token", details = ex.Message });
        }
    }

    /// <summary>
    /// Link Google account to existing user
    /// </summary>
    /// <param name="request">Google OAuth request with ID token and access token</param>
    /// <returns>Authentication response with updated JWT token</returns>
    [HttpPost("google/link")]
    [Authorize]
    public async Task<IActionResult> LinkGoogleAccount([FromBody] LinkGoogleAccountRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            // Extract Google ID from token (simplified)
            var googleId = "google_" + Guid.NewGuid().ToString("N")[..8]; // Simplified for demo
            var profilePictureUrl = "https://via.placeholder.com/150"; // Extract from token

            var result = await _authenticationService.LinkGoogleAccountAsync(
                userId, googleId, profilePictureUrl);
            
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Invalid Google token", details = ex.Message });
        }
    }

    /// <summary>
    /// Unlink Google account from user
    /// </summary>
    /// <returns>Success result</returns>
    [HttpPost("google/unlink")]
    [Authorize]
    public async Task<IActionResult> UnlinkGoogleAccount()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result = await _authenticationService.UnlinkGoogleAccountAsync(userId);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Failed to unlink Google account", details = ex.Message });
        }
    }
}
