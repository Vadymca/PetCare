namespace PetCare.Application.Features.Auth.Login;

using System.Security.Cryptography;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles the <see cref="LoginUserCommand"/> request to authenticate a user and generate tokens.
/// </summary>
public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly IUserService userService;
    private readonly IJwtService jwtService;
    private readonly ILogger<LoginUserCommandHandler> logger;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to query and manage users.</param>
    /// <param name="jwtService">
    /// The JWT service used to generate access and refresh tokens, and set cookies.</param>
    /// <param name="logger">The logger instance used to record diagnostic and operational messages.</param>
    /// <param name="httpContextAccessor">
    /// The HTTP context accessor used to access the current HTTP response for setting cookies.</param>
    /// <param name="mapper">The AutoMapper instance used for entity-to-DTO mapping.</param>
    public LoginUserCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        ILogger<LoginUserCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the <see cref="LoginUserCommand"/> request by validating the user's credentials
    /// and returning login response data with tokens.
    /// </summary>
    /// <param name="request">The login command containing the user's email and password.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
    /// with a <see cref="LoginResponseDto"/> containing authentication tokens and user information.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the email or password provided is invalid.
    /// </exception>
    public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Знаходимо користувача за email
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new InvalidOperationException("Невірний email.");
        }

        if (!user.EmailConfirmed)
        {
            return new LoginResponseDto(
               Status: "email_not_verified",
               Message: "Будь ласка, підтвердьте вашу електронну пошту перед входом.");
        }

        // Перевіряємо пароль
        var passwordValid = await this.userService.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new InvalidOperationException("Невірний пароль.");
        }

        // 2FA логіка
        var twoFaToken = Generate2FaToken();
        await this.userService.Save2FaTokenAsync(user.Id, twoFaToken, TimeSpan.FromMinutes(5));

        if (user.PhoneNumberConfirmed && !user.TwoFactorEnabled)
        {
            return new LoginResponseDto(
                Status: "2fa_required",
                Method: "sms",
                HiddenPhoneNumber: this.HidePhoneNumber(user.Phone!),
                TwoFaToken: twoFaToken,
                Message: "Необхідна двофакторна автентифікація. Будь ласка, пройдіть SMS перевірку перед входом.");
        }

        if ((!user.PhoneNumberConfirmed && user.TwoFactorEnabled) || (user.PhoneNumberConfirmed && user.TwoFactorEnabled))
        {
            return new LoginResponseDto(
                Status: "2fa_required",
                Method: "totp",
                TwoFaToken: twoFaToken,
                Message: "Необхідна двофакторна автентифікація. Будь ласка, пройдіть TOTP перевірку перед входом.");
        }

        // Оновлюємо LastLogin
        await this.userService.SetLastLoginAsync(user, DateTime.UtcNow);

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        // Створюємо UserDto
        var userDto = this.mapper.Map<UserDto>(user) with
        {
            Role = userRole,
        };

        // Генеруємо Access Token
        var accessToken = this.jwtService.GenerateAccessToken(user, roles);

        // Генеруємо Refresh Token
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Встановлюємо cookie для Refresh Token
        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        this.logger.LogInformation("Користувач {Email} увійшов, JWT збережено в cookie.", request.Email);

        return new LoginResponseDto(
            Status: "success",
            AccessToken: accessToken,
            User: userDto);
    }

    private static string Generate2FaToken(int length = 6)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = RandomNumberGenerator.GetBytes(length);
        var result = new char[length];

        for (int i = 0; i < length; i++)
        {
            // Вибираємо символ з chars за індексом
            result[i] = chars[bytes[i] % chars.Length];
        }

        return new string(result);
    }

    private string? HidePhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length < 7)
        {
            return phone;
        }

        var last2 = phone[^2..];
        var countryCode = phone.StartsWith("+380") ? "+380" : phone[..4];
        return $"{countryCode}*******{last2}";
    }
}