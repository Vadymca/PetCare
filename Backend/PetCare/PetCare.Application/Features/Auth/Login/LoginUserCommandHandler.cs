namespace PetCare.Application.Features.Auth.Login;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to query and manage users.</param>
    /// <param name="jwtService">
    /// The JWT service used to generate access and refresh tokens, and set cookies.</param>
    /// <param name="logger">The logger instance used to record diagnostic and operational messages.</param>
    /// /// <param name="httpContextAccessor">
    /// The HTTP context accessor used to access the current HTTP response for setting cookies.</param>
    public LoginUserCommandHandler(
        IUserService userService,
        IJwtService jwtService,
        ILogger<LoginUserCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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
        if (user == null)
        {
            throw new InvalidOperationException("Невірний email.");
        }

        if (!user.EmailConfirmed)
        {
            throw new InvalidOperationException("Будь ласка, підтвердьте вашу електронну пошту перед входом.");
        }

        // Перевіряємо пароль
        var passwordValid = await this.userService.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new InvalidOperationException("Невірний пароль.");
        }

        // Отримуємо ролі користувача
        var roles = await this.userService.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        // Створюємо UserDto
        var userDto = new UserDto(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.Phone,
            userRole,
            user.PostalCode);

        // Генеруємо Access Token
        var accessToken = this.jwtService.GenerateAccessToken(user);

        // Генеруємо Refresh Token
        var refreshToken = this.jwtService.GenerateRefreshToken(user.Id);

        // Встановлюємо cookie для Access Token
        this.jwtService.SetAccessTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            accessToken);

        // Встановлюємо cookie для Refresh Token
        this.jwtService.SetRefreshTokenCookie(
            this.httpContextAccessor.HttpContext!.Response,
            refreshToken);

        this.logger.LogInformation("Користувач {Email} увійшов, JWT збережено в cookie.", request.Email);

        return new LoginResponseDto(accessToken, refreshToken, userDto);
    }
}