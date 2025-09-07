namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles verification of a TOTP backup code for the current user.
/// </summary>
public sealed class VerifyTotpBackupCodeCommandHandler
    : IRequestHandler<VerifyTotpBackupCodeCommand, VerifyTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<VerifyTotpBackupCodeCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpBackupCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used for retrieving and validating users.</param>
    /// <param name="jwtService">The JWT service responsible for generating tokens and managing cookies.</param>
    /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is <c>null</c>.</exception>
    public VerifyTotpBackupCodeCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VerifyTotpBackupCodeCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<VerifyTotpResponseDto> Handle(VerifyTotpBackupCodeCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо поточного користувача
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify TOTP backup code.");
            return new VerifyTotpResponseDto(false, "Користувач не авторизований.", null, null);
        }

        // Перевіряємо код через сервіс
        var isValid = await this.userService.VerifyTotpBackupCodeAsync(user, request.Code);
        if (!isValid)
        {
            this.logger.LogWarning("Invalid TOTP backup code attempt for user {Email}", user.Email);
            return new VerifyTotpResponseDto(false, "Невірний резервний код.", null, null);
        }

        // Генеруємо токени та ставимо cookie
        var accessToken = this.jwtService.GenerateAccessToken(user);
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        this.jwtService.SetAccessTokenCookie(this.httpContextAccessor.HttpContext!.Response, accessToken);
        this.jwtService.SetRefreshTokenCookie(this.httpContextAccessor.HttpContext!.Response, refreshToken);

        this.logger.LogInformation("TOTP backup code successfully verified for user {Email}", user.Email);

        return new VerifyTotpResponseDto(true, "Резервний код успішно верифіковано.", accessToken, refreshToken);
    }
}
