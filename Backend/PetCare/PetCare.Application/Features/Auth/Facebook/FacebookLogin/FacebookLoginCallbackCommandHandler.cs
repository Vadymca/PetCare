namespace PetCare.Application.Features.Auth.Facebook.FacebookLogin;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles Facebook OAuth login callback logic.
/// </summary>
public sealed class FacebookLoginCallbackCommandHandler
    : IRequestHandler<FacebookLoginCallbackCommand, string>
{
    private readonly IFacebookAuthService facebookAuthService;
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<FacebookLoginCallbackCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FacebookLoginCallbackCommandHandler"/> class.
    /// </summary>
    /// <param name="facebookAuthService">The Facebook authentication service.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="jwtService">The JWT service.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="logger">The logger instance.</param>
    public FacebookLoginCallbackCommandHandler(
        IFacebookAuthService facebookAuthService,
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<FacebookLoginCallbackCommandHandler> logger)
    {
        this.facebookAuthService = facebookAuthService ?? throw new ArgumentNullException(nameof(facebookAuthService));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<string> Handle(FacebookLoginCallbackCommand request, CancellationToken cancellationToken)
    {
        var httpContext = this.httpContextAccessor.HttpContext!;
        this.logger.LogInformation("Handling Facebook login callback. Code: {Code}, State: {State}", request.Code, request.State);

        // Отримуємо access token
        var redirectUri = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/auth/facebook/callback";
        var accessToken = await this.facebookAuthService.GetAccessTokenAsync(request.Code, redirectUri);

        // Отримуємо дані користувача
        var fbUser = await this.facebookAuthService.GetUserInfoAsync(accessToken);

        // Перевіряємо користувача
        var user = await this.userService.FindByEmailAsync(fbUser.Email)
                   ?? await this.userService.CreateUserFromFacebookAsync(fbUser);

        // Отримуємо ролі
        var roles = await this.userService.GetRolesAsync(user);

        // Генеруємо refresh token і записуємо в cookie
        var jwtRefreshToken = this.jwtService.GenerateRefreshToken(user.Id);
        this.jwtService.SetRefreshTokenCookie(httpContext.Response, jwtRefreshToken);

        this.logger.LogInformation("User {Email} successfully logged in via Facebook.", user.Email);

        // Редірект на фронтенд
        return "https://localhost:4200";
    }
}
