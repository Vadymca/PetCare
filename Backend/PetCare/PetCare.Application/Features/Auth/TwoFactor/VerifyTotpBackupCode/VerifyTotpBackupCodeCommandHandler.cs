namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

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
/// Handles verification of a TOTP backup code for the current user.
/// </summary>
public sealed class VerifyTotpBackupCodeCommandHandler
    : IRequestHandler<VerifyTotpBackupCodeCommand, VerifyTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<VerifyTotpBackupCodeCommandHandler> logger;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpBackupCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used for retrieving and validating users.</param>
    /// <param name="jwtService">The JWT service responsible for generating tokens and managing cookies.</param>
    /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is <c>null</c>.</exception>
    public VerifyTotpBackupCodeCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VerifyTotpBackupCodeCommandHandler> logger,
        IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<VerifyTotpResponseDto> Handle(VerifyTotpBackupCodeCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо поточного користувача
        var user = await this.userService.GetUserByTwoFaTokenAsync(request.TwoFaToken);
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify TOTP backup code.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        // Перевіряємо код через сервіс
        var isValid = await this.userService.VerifyTotpBackupCodeAsync(user, request.Code);
        if (!isValid)
        {
            this.logger.LogWarning("Invalid TOTP backup code attempt for user {Email}", user.Email);
            throw new InvalidOperationException("Невірний резервний код.");
        }

        // Створюємо UserDto
        var userDto = this.mapper.Map<UserDto>(user);

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        // Генеруємо токени та ставимо cookie
        var accessToken = this.jwtService.GenerateAccessToken(user, roles);
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        this.jwtService.SetRefreshTokenCookie(this.httpContextAccessor.HttpContext!.Response, refreshToken);

        this.logger.LogInformation("TOTP backup code successfully verified for user {Email}", user.Email);

        return new VerifyTotpResponseDto(
            Success: true,
            Message: "Резервний код успішно верифіковано.",
            AccessToken: accessToken,
            User: userDto);
    }
}
