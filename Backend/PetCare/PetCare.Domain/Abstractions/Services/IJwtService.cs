namespace PetCare.Domain.Abstractions.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PetCare.Domain.Aggregates;

/// <summary>
/// Service for generating JWT tokens for users, including access and refresh tokens.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a signed access JWT token for the specified user with roles.
    /// </summary>
    /// <param name="user">Application user.</param>
    /// <param name="roles">Collection of roles assigned to the user.</param>
    /// <returns>Serialized JWT access token.</returns>
    string GenerateAccessToken(User user, IEnumerable<string> roles);

    /// <summary>
    /// Generates a signed refresh JWT token for the specified user ID.
    /// </summary>
    /// <param name="userId">Application user ID.</param>
    /// <returns>Serialized JWT refresh token.</returns>
    string GenerateRefreshToken(Guid userId);

    /// <summary>
    /// Writes the access JWT into an HTTP-only cookie.
    /// </summary>
    /// <param name="response">HTTP response to append the cookie to.</param>
    /// <param name="token">Serialized JWT access token.</param>
    void SetAccessTokenCookie(HttpResponse response, string token);

    /// <summary>
    /// Writes the refresh JWT into an HTTP-only cookie.
    /// </summary>
    /// <param name="response">HTTP response to append the cookie to.</param>
    /// <param name="token">Serialized JWT refresh token.</param>
    void SetRefreshTokenCookie(HttpResponse response, string token);

    /// <summary>
    /// Removes both access and refresh JWT cookies.
    /// </summary>
    /// <param name="response">HTTP response.</param>
    void ClearCookies(HttpResponse response);

    /// <summary>
    /// Validates the JWT and returns a <see cref="ClaimsPrincipal"/> when valid.
    /// </summary>
    /// <param name="token">Serialized JWT.</param>
    /// <returns>Claims principal or <c>null</c> if invalid.</returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Validates the JWT and returns a <see cref="ClaimsPrincipal"/> when valid.
    /// </summary>
    /// <param name="token">Serialized JWT.</param>
    /// <returns>Claims principal or <c>null</c> if invalid.</returns>
    ClaimsPrincipal? ValidateRefreshToken(string token);
}
