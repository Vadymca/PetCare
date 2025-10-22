namespace PetCare.Application.Features.Auth.Google.GoogleLogin;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles Google OAuth login callback logic.
/// </summary>
public sealed class GoogleLoginCallbackCommandHandler
    : IRequestHandler<GoogleLoginCallbackCommand, string>
{
    private readonly IGoogleAuthService googleAuthService;
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<GoogleLoginCallbackCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleLoginCallbackCommandHandler"/> class.
    /// </summary>
    /// <param name="googleAuthService">The service for interacting with Google OAuth.</param>
    /// <param name="userService">The service for managing application users.</param>
    /// <param name="jwtService">The service for generating JWT tokens and managing cookies.</param>
    /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
    /// <param name="logger">The logger instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if any required service is null.</exception>
    public GoogleLoginCallbackCommandHandler(
        IGoogleAuthService googleAuthService,
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GoogleLoginCallbackCommandHandler> logger)
    {
        this.googleAuthService = googleAuthService ?? throw new ArgumentNullException(nameof(googleAuthService));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<string> Handle(GoogleLoginCallbackCommand request, CancellationToken cancellationToken)
    {
        var httpContext = this.httpContextAccessor.HttpContext!;
        this.logger.LogInformation("Handling Google login callback. Code: {Code}, State: {State}", request.Code, request.State);

        // Отримуємо access token
        var accessToken = await this.googleAuthService.GetAccessTokenAsync(request.Code);

        // Отримуємо дані користувача
        var googleUser = await this.googleAuthService.GetUserInfoAsync(accessToken);

        // Перевіряємо користувача
        var user = await this.userService.FindByEmailAsync(googleUser.Email)
                   ?? await this.userService.CreateUserFromGoogleAsync(googleUser);

        // Отримуємо ролі
        var roles = await this.userService.GetRolesAsync(user);

        // Генеруємо refresh token і записуємо в cookie
        var jwtRefreshToken = this.jwtService.GenerateRefreshToken(user.Id);
        this.jwtService.SetRefreshTokenCookie(httpContext.Response, jwtRefreshToken);

        this.logger.LogInformation("User {Email} successfully logged in via Google.", user.Email);

        // Редірект на фронтенд
        return "https://localhost:4200";
    }
}
