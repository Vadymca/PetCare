namespace PetCare.Application.Features.Auth.Register;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles the <see cref="RegisterUserCommand"/> request.
/// Responsible for creating a new user, assigning the default role,
/// and returning the corresponding <see cref="UserDto"/>.
/// </summary>
public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserService userService;
    private readonly IEmailService emailService;
    private readonly IEmailTemplateRenderer templateRenderer;
    private readonly ILogger<RegisterUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to perform user-related operations.</param>
    /// <param name="emailService">
    /// The service used to send emails to users, including confirmation and notification messages.</param>
    /// <param name="templateRenderer">
    /// The service responsible for rendering Razor email templates to HTML strings.</param>
    /// <param name="logger">The logger instance used to record diagnostic and operational messages.</param>
    public RegisterUserCommandHandler(
        IUserService userService,
        IEmailService emailService,
        IEmailTemplateRenderer templateRenderer,
        ILogger<RegisterUserCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
    }

    /// <summary>
    /// Handles the <see cref="RegisterUserCommand"/> request by creating a new user,
    /// generating an email confirmation token, and returning the corresponding <see cref="UserDto"/>.
    /// </summary>
    /// <param name="request">The register command containing user details such as email, password, and personal information.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
    /// with a <see cref="UserDto"/> containing the registered user's details.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a user with the specified email already exists.
    /// </exception>
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Перевіряємо унікальність email
        var existingUser = await this.userService.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Користувач із email {request.Email} уже зареєстрований.");
        }

        // Створюємо користувача через UserService
        var user = await this.userService.CreateUserAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.PostalCode);

        // Генеруємо токен підтвердження email
        var emailConfirmationToken = await this.userService.GenerateEmailConfirmationTokenAsync(user);

        // Формуємо посилання на підтвердження email
        var confirmationUrl = $"http://localhost:4200/verify-email?token={emailConfirmationToken}";

        // Підготовка email
        var subject = "Підтвердження Email для PetCare";
        var htmlBody = await this.templateRenderer.RenderAsync(
            "PetCare.Application.EmailTemplates.ConfirmEmailTemplate.cshtml",
            confirmationUrl);

        // Відправка email через MailKit
        await this.emailService.SendEmailAsync(user.Email!, subject, htmlBody);

        this.logger.LogInformation(
            "Згенеровано токен підтвердження email для користувача {Email}. Токен: {Token}",
            request.Email,
            emailConfirmationToken);

        this.logger.LogInformation("Користувач {Email} успішно зареєстрований. Очікується підтвердження email.", request.Email);

        // Повертаємо DTO
        return new UserDto(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.Phone,
            "User",
            user.PostalCode);
    }
}