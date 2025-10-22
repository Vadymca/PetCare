namespace PetCare.Application.Features.Auth.ResendVerification;

using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="ResendVerificationCommand"/> request.
/// Responsible for resending the verification email to the user.
/// </summary>
public sealed class ResendVerificationCommandHandler : IRequestHandler<ResendVerificationCommand, ResendVerificationResponseDto>
{
    private readonly IUserService userService;
    private readonly IEmailService emailService;
    private readonly IEmailTemplateRenderer templateRenderer;
    private readonly IEmailAssetProvider emailAssetProvider;
    private readonly ILogger<ResendVerificationCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResendVerificationCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for user management.</param>
    /// <param name="emailService">Service for sending emails.</param>
    /// <param name="templateRenderer">Service for rendering email templates.</param>
    /// <param name="emailAssetProvider">Service for loading and providing email assets (e.g., logo).</param>
    /// <param name="logger">Logger instance for logging activities.</param>
    public ResendVerificationCommandHandler(
        IUserService userService,
        IEmailService emailService,
        IEmailTemplateRenderer templateRenderer,
        IEmailAssetProvider emailAssetProvider,
        ILogger<ResendVerificationCommandHandler> logger)
    {
        this.userService = userService;
        this.emailService = emailService;
        this.templateRenderer = templateRenderer;
        this.emailAssetProvider = emailAssetProvider;
        this.logger = logger;
    }

    /// <summary>
    /// Handles the command to resend a verification email.
    /// </summary>
    /// <param name="request">The command containing the user's email.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>
    /// A boolean indicating whether the email was successfully resent
    /// or skipped if the email was already confirmed.
    /// </returns>
    public async Task<ResendVerificationResponseDto> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            this.logger.LogWarning("Resend verification requested for non-existing email: {Email}", request.Email);
            throw new InvalidOperationException("Користувача з таким email не існує.");
        }

        if (user.EmailConfirmed)
        {
            this.logger.LogInformation("Email already confirmed for user: {Email}", request.Email);
            return new ResendVerificationResponseDto(
                 Success: true,
                 Message: "Email вже підтверджений.");
        }

        var token = await this.userService.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var confirmationUrl = $"https://localhost:4200/verify-email?token={encodedToken}&email={user.Email}";

        var subject = "Підтвердження Email для Добродій";

        var model = new ConfirmEmailViewModel(user.FirstName, confirmationUrl);

        var htmlBody = await this.templateRenderer.RenderAsync(
            "PetCare.Application.EmailTemplates.ConfirmEmailTemplate.cshtml",
            model);

        await this.emailService.SendEmailAsync(user.Email!, subject, htmlBody);

        this.logger.LogInformation("Resent verification email for user: {Email}", request.Email);
        return new ResendVerificationResponseDto(
            Success: true,
            Message: "Лист з підтвердженням успішно відправлений.");
    }
}
