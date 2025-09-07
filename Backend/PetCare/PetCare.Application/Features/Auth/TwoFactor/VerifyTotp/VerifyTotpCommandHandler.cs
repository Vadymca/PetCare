namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles the verification of TOTP codes during login.
/// </summary>
public sealed class VerifyTotpCommandHandler : IRequestHandler<VerifyTotpCommand, VerifyTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<VerifyTotpCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used for retrieving and validating user information.</param>
    /// <param name="jwtService">The JWT service responsible for generating tokens and managing cookies.</param>
    /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the required dependencies (<paramref name="userService"/>,
    /// <paramref name="jwtService"/>, <paramref name="httpContextAccessor"/>, <paramref name="logger"/>)
    /// is <c>null</c>.
    /// </exception>
    public VerifyTotpCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VerifyTotpCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<VerifyTotpResponseDto> Handle(VerifyTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user is null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify TOTP.");
            return new VerifyTotpResponseDto(false, "Користувач не авторизований.", null, null);
        }

        var isValid = await this.userService.VerifyTotpCodeAsync(user, request.Code);
        if (!isValid)
        {
            return new VerifyTotpResponseDto(false, "Невірний TOTP код.", null, null);
        }

        var accessToken = this.jwtService.GenerateAccessToken(user);
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Set cookies
        this.jwtService.SetAccessTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            accessToken);

        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        this.logger.LogInformation("2FA успішно пройдена користувачем {Email}", user.Email);

        return new VerifyTotpResponseDto(true, "TOTP верифіковано успішно.", accessToken, refreshToken);
    }
}
