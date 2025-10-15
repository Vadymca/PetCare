namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles the verification of TOTP codes during login.
/// </summary>
public sealed class VerifyTotpCommandHandler : IRequestHandler<VerifyTotpCommand, VerifyTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<VerifyTotpCommandHandler> logger;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used for retrieving and validating user information.</param>
    /// <param name="jwtService">The JWT service responsible for generating tokens and managing cookies.</param>
    /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the required dependencies (<paramref name="userService"/>,
    /// <paramref name="jwtService"/>, <paramref name="httpContextAccessor"/>, <paramref name="logger"/>)
    /// is <c>null</c>.
    /// </exception>
    public VerifyTotpCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VerifyTotpCommandHandler> logger,
        IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<VerifyTotpResponseDto> Handle(VerifyTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetUserByTwoFaTokenAsync(request.TwoFaToken);
        if (user is null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify TOTP.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        var isValid = await this.userService.VerifyTotpCodeAsync(user, request.Code);
        if (!isValid)
        {
            throw new InvalidOperationException("Невірний TOTP код.");
        }

        // Створюємо UserDto
        var userDto = this.mapper.Map<UserDto>(user);

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        var accessToken = this.jwtService.GenerateAccessToken(user, roles);
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Встановлюємо cookie для Refresh Token
        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        this.logger.LogInformation("2FA успішно пройдена користувачем {Email}", user.Email);

        return new VerifyTotpResponseDto(
             Success: true,
             Message: "TOTP верифіковано успішно.",
             AccessToken: accessToken,
             User: userDto);
    }
}
