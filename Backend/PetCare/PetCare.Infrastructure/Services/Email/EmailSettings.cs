namespace PetCare.Infrastructure.Services.Email;

/// <summary>
/// Represents the configuration settings required for sending emails via SMTP.
/// </summary>
public sealed class EmailSettings
{
    /// <summary>
    /// Gets or sets the SMTP server address (e.g., "smtp.gmail.com").
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the port number used by the SMTP server (e.g., 587 for STARTTLS).
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the display name of the email sender (e.g., "PetCare").
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the sender (e.g., "petcare.app.noreply@gmail.com").
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
