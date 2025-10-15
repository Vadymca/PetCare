namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Email service abstraction.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email message.
    /// </summary>
    /// <param name="to">Recipient email.</param>
    /// <param name="subject">Message subject.</param>
    /// <param name="body">Message body (HTML supported).</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendEmailAsync(string to, string subject, string body);
}
