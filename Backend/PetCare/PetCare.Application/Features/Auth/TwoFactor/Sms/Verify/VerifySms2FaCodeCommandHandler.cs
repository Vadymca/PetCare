namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

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
/// Handles verification of the SMS 2FA code for the current user.
/// </summary>
public sealed class VerifySms2FaCodeCommandHandler : IRequestHandler<VerifySms2FaCodeCommand, VerifySms2FaCodeResponseDto>
{
    private readonly IUserService userService;
    private readonly ISms2FaService sms2FaService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<VerifySms2FaCodeCommandHandler> logger;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySms2FaCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service to manage user operations.</param>
    /// <param name="sms2FaService">Service to handle SMS 2FA operations.</param>
    /// <param name="jwtService">
    /// The JWT service used to generate access and refresh tokens, and set cookies.</param>
    /// <param name="httpContextAccessor">
    /// The HTTP context accessor used to access the current HTTP response for setting cookies.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
    public VerifySms2FaCodeCommandHandler(
        IUserService userService,
        ISms2FaService sms2FaService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VerifySms2FaCodeCommandHandler> logger,
        IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.sms2FaService = sms2FaService ?? throw new ArgumentNullException(nameof(sms2FaService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<VerifySms2FaCodeResponseDto> Handle(VerifySms2FaCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetUserByTwoFaTokenAsync(request.TwoFaToken);
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify SMS 2FA code.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        var verified = await this.sms2FaService.VerifySetupCodeAsync(user.Id.ToString(), request.Code);
        if (!verified)
        {
            this.logger.LogWarning("Invalid SMS 2FA code attempt for user {UserId}", user.Id);
            throw new UnauthorizedAccessException("Невірний код.");
        }

        if (!user.PhoneNumberConfirmed)
        {
            await this.userService.ConfirmPhoneNumberAsync(user);
        }

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);

        // Генеруємо Access Token
        var accessToken = this.jwtService.GenerateAccessToken(user, roles);

        // Генеруємо Refresh Token
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Встановлюємо cookie для Refresh Token
        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        // Створюємо UserDto
        var userDto = this.mapper.Map<UserDto>(user);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        this.logger.LogInformation("SMS 2FA code successfully verified for user {UserId}", user.Id);
        return new VerifySms2FaCodeResponseDto(
            Success: true,
            Message: "SMS 2FA код успішно верифіковано.",
            AccessToken: accessToken,
            User: userDto);
    }
}
