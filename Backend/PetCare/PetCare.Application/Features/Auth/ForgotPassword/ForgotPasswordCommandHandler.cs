namespace PetCare.Application.Features.Auth.ForgotPassword;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles sending a password reset token to the user's email (mock).
/// </summary>
public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponseDto>
{
    private readonly IUserService userService;
    private readonly IEmailService emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for user operations.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="emailService">Service for sending emails.</param>
    /// <exception cref="ArgumentNullException">Thrown if a dependency is <c>null</c>.</exception>
    public ForgotPasswordCommandHandler(
        IUserService userService,
        ILogger<ForgotPasswordCommandHandler> logger,
        IEmailService emailService)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentException(nameof(logger));
        this.emailService = emailService ?? throw new ArgumentException(nameof(emailService));
    }

    /// <summary>
    /// Handles the <see cref="ForgotPasswordCommand"/>.
    /// Generates a password reset token and sends it to the user's email if the user exists.
    /// </summary>
    /// <param name="request">The command containing the email to send reset instructions to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A <see cref="ForgotPasswordResponseDto"/> indicating whether the request was handled successfully.
    /// </returns>
    public async Task<ForgotPasswordResponseDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // Не викидаємо помилку, щоб не видати інформацію про існування користувача
            this.logger.LogWarning("ForgotPassword requested for non-existing email: {Email}", request.Email);
            return new ForgotPasswordResponseDto(true, "Якщо email існує, на нього надіслано лист.");
        }

        // Генеруємо токен для скидання пароля
        var token = await this.userService.GeneratePasswordResetTokenAsync(user);

        // Формуємо посилання для фронтенду
        var resetUrl = $"http://localhost:4200/reset-password?token={token}&email={user.Email}";

        // Відправка мок листа
        var subject = "Скидання пароля для PetCare";
        var body = $"Посилання для скидання пароля: {resetUrl}";
        await this.emailService.SendEmailAsync(user.Email!, subject, body);

        this.logger.LogInformation("Reset password email sent to {Email}", user.Email);

        return new ForgotPasswordResponseDto(true, "Якщо email існує, на нього надіслано лист.");
    }
}
