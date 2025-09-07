namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Abstraction of the SMS sending service..
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// Sends an SMS to the specified number in E.164 format.
    /// </summary>
    /// <param name="toPhoneE164">Recipient's number in E.164 format (e.g., +14195550123).</param>
    /// <param name="message">Text of the message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><c>true</c>, if sent successfully; otherwise <c>false</c>.</returns>
    Task<bool> SendAsync(string toPhoneE164, string message, CancellationToken cancellationToken = default);
}