namespace PetCare.Infrastructure.Services.Email;

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PetCare.Application.Interfaces;

/// <summary>
/// Implementation of email service using SMTP.
/// </summary>
public sealed class EmailService : IEmailService
{
    private readonly EmailSettings settings;
    private readonly IEmailAssetProvider assetProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="options">
    /// The <see cref="IOptions{EmailSettings}"/> containing configuration for SMTP email sending,
    /// including server, port, sender name, sender email, username, and password.
    /// </param>
    /// <param name="assetProvider">Service for providing embedded email assets (e.g., logo).</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is null or <see cref="EmailSettings"/> is null.
    /// </exception>
    public EmailService(IOptions<EmailSettings> options, IEmailAssetProvider assetProvider)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        this.assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(this.settings.SenderName, this.settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        // Create the body builder
        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody,
        };

        // Embed the logo as a linked resource with ContentId "petcare-logo"
        var logoBytes = Convert.FromBase64String(this.assetProvider.GetLogoBase64().Split(',')[1]);
        var logo = builder.LinkedResources.Add("logo.png", logoBytes);
        logo.ContentId = "petcare-logo";
        logo.ContentType.MediaType = "image";
        logo.ContentType.MediaSubtype = "png";
        logo.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(this.settings.SmtpServer, this.settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(this.settings.Username, this.settings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
