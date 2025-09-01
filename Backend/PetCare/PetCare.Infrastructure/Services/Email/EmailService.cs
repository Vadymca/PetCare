namespace PetCare.Infrastructure.Services.Email;

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PetCare.Application.Interfaces;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Implementation of email service using SMTP.
/// </summary>
public sealed class EmailService : IEmailService
{
    private readonly EmailSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="options">
    /// The <see cref="IOptions{EmailSettings}"/> containing configuration for SMTP email sending,
    /// including server, port, sender name, sender email, username, and password.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is null or <see cref="EmailSettings"/> is null.
    /// </exception>
    public EmailService(IOptions<EmailSettings> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(this.settings.SenderName, this.settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        var body = new TextPart(TextFormat.Html)
        {
            Text = htmlBody,
        };
        body.ContentTransferEncoding = ContentEncoding.QuotedPrintable;
        body.ContentType.Charset = Encoding.UTF8.WebName;
        email.Body = body;

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(this.settings.SmtpServer, this.settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(this.settings.Username, this.settings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
