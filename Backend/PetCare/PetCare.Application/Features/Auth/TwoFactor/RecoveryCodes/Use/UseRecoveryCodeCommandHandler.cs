namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

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
/// Handles the usage of a recovery code for two-factor authentication.
/// </summary>
public sealed class UseRecoveryCodeCommandHandler : IRequestHandler<UseRecoveryCodeCommand, UseRecoveryCodeResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<UseRecoveryCodeCommandHandler> logger;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UseRecoveryCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to manage user operations and 2FA functionality.</param>
    /// <param name="jwtService">
    /// The JWT service used to generate access and refresh tokens, and set cookies.</param>
    /// <param name="httpContextAccessor">
    /// The HTTP context accessor used to access the current HTTP response for setting cookies.</param>
    /// <param name="logger">The logger used to log information, warnings, and errors.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="userService"/> or <paramref name="logger"/> is null.
    /// </exception>
    public UseRecoveryCodeCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UseRecoveryCodeCommandHandler> logger,
        IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<UseRecoveryCodeResponseDto> Handle(UseRecoveryCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetUserByTwoFaTokenAsync(request.TwoFaToken);
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to use recovery code.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        var success = await this.userService.RedeemRecoveryCodeAsync(user, request.Code);
        if (!success)
        {
            throw new InvalidOperationException("Невірний або вже використаний код відновлення.");
        }

        // Створюємо UserDto
        var userDto = this.mapper.Map<UserDto>(user);

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        // Генеруємо Access Token
        var accessToken = this.jwtService.GenerateAccessToken(user, roles);

        // Генеруємо Refresh Token
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Встановлюємо cookie для Refresh Token
        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        return new UseRecoveryCodeResponseDto(
             Success: true,
             Message: "Код відновлення прийнято.",
             AccessToken: accessToken,
             User: userDto);
    }
}
