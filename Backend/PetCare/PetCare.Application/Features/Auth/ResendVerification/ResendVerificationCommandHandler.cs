namespace PetCare.Application.Features.Auth.ResendVerification;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="ResendVerificationCommand"/> request.
/// Responsible for resending the verification email to the user.
/// </summary>
public sealed class ResendVerificationCommandHandler : IRequestHandler<ResendVerificationCommand, bool>
{
    private readonly IUserService userService;
    private readonly IEmailService emailService;
    private readonly IEmailTemplateRenderer templateRenderer;
    private readonly ILogger<ResendVerificationCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResendVerificationCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for user management.</param>
    /// <param name="emailService">Service for sending emails.</param>
    /// <param name="templateRenderer">Service for rendering email templates.</param>
    /// <param name="logger">Logger instance for logging activities.</param>
    public ResendVerificationCommandHandler(
        IUserService userService,
        IEmailService emailService,
        IEmailTemplateRenderer templateRenderer,
        ILogger<ResendVerificationCommandHandler> logger)
    {
        this.userService = userService;
        this.emailService = emailService;
        this.templateRenderer = templateRenderer;
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
    public async Task<bool> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            this.logger.LogWarning("Resend verification requested for non-existing email: {Email}", request.Email);
            return false;
        }

        if (user.EmailConfirmed)
        {
            this.logger.LogInformation("Email already confirmed for user: {Email}", request.Email);
            return true;
        }

        var token = await this.userService.GenerateEmailConfirmationTokenAsync(user);
        var confirmationUrl = $"http://localhost:4200/verify-email?token={token}";

        var subject = "Підтвердження Email для PetCare";

        var htmlBody = await this.templateRenderer.RenderAsync(
            "PetCare.Application.EmailTemplates.ConfirmEmailTemplate.cshtml",
            confirmationUrl);

        await this.emailService.SendEmailAsync(user.Email!, subject, htmlBody);

        this.logger.LogInformation("Resent verification email for user: {Email}", request.Email);
        return true;
    }
}
