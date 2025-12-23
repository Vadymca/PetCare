namespace PetCare.Application.Features.Auth.Social.ExchangeMiniToken;

using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles mini token exchange and sets refresh token cookie.
/// </summary>
public sealed class ExchangeMiniTokenCommandHandler
    : IRequestHandler<ExchangeMiniTokenCommand>
{
    private readonly IMemoryCache memoryCache;
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<ExchangeMiniTokenCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeMiniTokenCommandHandler"/> class.
    /// </summary>
    /// <param name="memoryCache">Memory cache instance.</param>
    /// <param name="userService">User service instance.</param>
    /// <param name="jwtService">JWT service instance.</param>
    /// <param name="httpContextAccessor">HTTP context accessor instance.</param>
    /// <param name="logger">Logger instance.</param>
    public ExchangeMiniTokenCommandHandler(
        IMemoryCache memoryCache,
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ExchangeMiniTokenCommandHandler> logger)
    {
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task Handle(
        ExchangeMiniTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (!this.memoryCache.TryGetValue<Guid>(request.Token, out var userId))
        {
            this.logger.LogWarning(
                "Invalid or expired mini token: {MiniToken}",
                request.Token);

            throw new InvalidOperationException("Соціальний токен недійсний або протермінований.");
        }

        // одноразовість
        this.memoryCache.Remove(request.Token);

        var user = await this.userService.FindByIdAsync(userId)
                   ?? throw new InvalidOperationException("Користувача не знайдено.");

        var httpContext = this.httpContextAccessor.HttpContext
                          ?? throw new InvalidOperationException("Контекст HTTP недоступний.");

        // Генеруємо refresh token
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Зберігаємо refresh token в cookie
        this.jwtService.SetRefreshTokenCookie(
            httpContext.Response,
            refreshToken);

        this.logger.LogInformation(
            "Refresh token cookie successfully issued for user {UserId}",
            user.Id);
    }
}
