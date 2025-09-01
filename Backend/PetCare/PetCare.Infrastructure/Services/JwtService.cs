namespace PetCare.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// JWT token generation service.
/// </summary>
/// <summary>
/// Default implementation of <see cref="IJwtService"/> based on symmetric signing key.
/// </summary>
public sealed class JwtService : IJwtService
{
    private readonly ILogger<JwtService> logger;
    private readonly string secretKey;
    private readonly string issuer;
    private readonly string audience;
    private readonly int expirationMinutes;
    private readonly int refreshExpirationDays;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="configuration">Application configuration.</param>
    /// <param name="logger">Logger instance.</param>
    public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
    {
        this.logger = logger;

        secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                     ?? configuration["JwtSettings:SecretKey"]
                     ?? throw new InvalidOperationException("JWT SecretKey не встановлено");

        issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                      ?? configuration["JwtSettings:Issuer"]
                      ?? "PetCare.Api";

        audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                        ?? configuration["JwtSettings:Audience"]
                        ?? "PetCare.Client";

        expirationMinutes = int.Parse(
            Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES")
            ?? configuration["JwtSettings:ExpirationMinutes"]
            ?? "60");

        refreshExpirationDays = int.Parse(
           Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRATION_DAYS")
           ?? configuration["JwtSettings:RefreshExpirationDays"]
           ?? "7");
    }

    /// <summary>
    /// Generates a short-lived access token containing user claims.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <returns>Serialized JWT access token.</returns>
    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
            new Claim("phone", user.Phone ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        logger.LogInformation("JWT згенеровано для користувача {UserId}", user.Id);
        return tokenString;
    }

    /// <summary>
    /// Generates a long-lived refresh token with minimal payload.
    /// </summary>
    /// <param name="userId">The user ID for whom the refresh token is generated.</param>
    /// <returns>Serialized JWT refresh token.</returns>
    public string GenerateRefreshToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Використовуємо ClaimsIdentity, щоб явно передати claims
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("typ", "refresh"),
        };

        var identity = new ClaimsIdentity(claims);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: identity.Claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(refreshExpirationDays),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Writes the access token into an HTTP-only cookie.
    /// </summary>
    /// <param name="response">HTTP response to append the cookie to.</param>
    /// <param name="token">Serialized JWT access token.</param>
    public void SetAccessTokenCookie(HttpResponse response, string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Path = "/",
        };

        response.Cookies.Append("access_token", token, options);
        logger.LogInformation("Access token cookie встановлено, екcп.: {Expiration}", options.Expires);
    }

    /// <summary>
    /// Writes the refresh token into an HTTP-only cookie.
    /// </summary>
    /// <param name="response">HTTP response to append the cookie to.</param>
    /// <param name="token">Serialized JWT refresh token.</param>
    public void SetRefreshTokenCookie(HttpResponse response, string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(refreshExpirationDays),
            Path = "/",
        };

        response.Cookies.Append("refresh_token", token, options);
        logger.LogInformation("Refresh token cookie встановлено, екcп.: {Expiration}", options.Expires);
    }

    /// <summary>
    /// Deletes both access and refresh cookies.
    /// </summary>
    /// <param name="response">HTTP response.</param>
    public void ClearCookies(HttpResponse response)
    {
        response.Cookies.Delete("access_token");
        response.Cookies.Delete("refresh_token");

        logger.LogInformation("JWT cookies очищено");
    }

    /// <summary>
    /// Validates a JWT token and returns the corresponding <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="token">Serialized JWT.</param>
    /// <returns>Claims principal if valid; otherwise, null.</returns>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var principal = handler.ValidateToken(token, parameters, out _);
            return principal;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Валідація JWT неуспішна");
            return null;
        }
    }

    /// <summary>
    /// Validates a JWT token and returns the corresponding <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="token">Serialized JWT.</param>
    /// <returns>Claims principal if valid; otherwise, null.</returns>
    public ClaimsPrincipal? ValidateRefreshToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var principal = handler.ValidateToken(token, parameters, out _);

            // Читаємо claims безпосередньо з JWT
            var jwtToken = handler.ReadJwtToken(token);
            var typ = jwtToken.Claims.FirstOrDefault(c => c.Type == "typ")?.Value;
            if (typ != "refresh")
            {
                return null;
            }

            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(subClaim))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
