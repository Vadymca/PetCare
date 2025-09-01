namespace PetCare.Application.Features.Auth.Refresh;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles refreshing of JWT access and refresh tokens using refresh token from cookie.
/// </summary>
public sealed class RefreshUserCommandHandler : IRequestHandler<RefreshUserCommand, LoginResponseDto>
{
    private readonly IJwtService jwtService;
    private readonly IUserService userService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<RefreshUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshUserCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtService">Service for generating and validating JWT tokens.</param>
    /// <param name="userService">Service for accessing user information.</param>
    /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
    /// <param name="logger">Logger instance for diagnostic information.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies are null.</exception>
    public RefreshUserCommandHandler(
        IJwtService jwtService,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RefreshUserCommandHandler> logger)
    {
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the refresh process for a user by validating the refresh token,
    /// generating new access and refresh tokens, and updating the cookies.
    /// </summary>
    /// <param name="request">The refresh command request.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>A <see cref="LoginResponseDto"/> containing new access and refresh tokens along with user details.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the refresh token is missing, invalid, or the user cannot be found.
    /// </exception>
    public async Task<LoginResponseDto> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var context = this.httpContextAccessor.HttpContext;
        if (context is null)
        {
            this.logger.LogError("HttpContext is null");
            throw new InvalidOperationException("Невірний refresh token.");
        }

        var refreshToken = context.Request.Cookies["refresh_token"];
        this.logger.LogInformation("Refresh token from cookie: {Token}", refreshToken ?? "NULL");

        if (string.IsNullOrEmpty(refreshToken))
        {
            this.logger.LogWarning("No refresh token found in cookies");
            throw new InvalidOperationException("Невірний refresh token.");
        }

        // Розбираємо токен для дебагу
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
            var claimsLog = string.Join(", ", jwtToken.Claims.Select(c => $"{c.Type}={c.Value}"));
            this.logger.LogInformation("Claims in refresh token: {Claims}", claimsLog);
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to parse refresh token");
        }

        var principal = this.jwtService.ValidateRefreshToken(refreshToken);
        if (principal is null)
        {
            this.logger.LogWarning("ValidateRefreshToken returned null for token: {Token}", refreshToken);
            throw new InvalidOperationException("Невірний refresh token.");
        }

        // Забираємо userId з NameIdentifier або Sub
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            this.logger.LogWarning("Refresh token sub claim is invalid: {Sub}", userIdClaim ?? "NULL");
            throw new InvalidOperationException("Невірний refresh token.");
        }

        var user = await this.userService.FindByIdAsync(userId);
        if (user is null)
        {
            this.logger.LogWarning("User not found with id: {UserId}", userId);
            throw new InvalidOperationException("Користувач не знайдений.");
        }

        // Генеруємо нові токени
        var newAccessToken = this.jwtService.GenerateAccessToken(user);
        var newRefreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Оновлюємо cookie
        this.jwtService.SetAccessTokenCookie(context.Response, newAccessToken);
        this.jwtService.SetRefreshTokenCookie(context.Response, newRefreshToken);

        this.logger.LogInformation("Refresh token successfully validated for user {UserId}", user.Id);

        var userDto = new UserDto(user.Id, user.Email!, user.FirstName, user.LastName, user.Phone, "User", user.PostalCode);
        return new LoginResponseDto(newAccessToken, newRefreshToken, userDto);
    }
}
