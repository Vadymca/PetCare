namespace PetCare.Application.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Represents a service responsible for managing SMS-based two-factor authentication (2FA) for users.
/// </summary>
public interface ISms2FaService
{
    /// <summary>
    /// Generates a verification code for SMS 2FA setup and sends it to the specified phone number.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the code is generated.</param>
    /// <param name="phoneNumber">The phone number to which the verification code should be sent.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the SMS was successfully sent; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SendSetupCodeAsync(string userId, string phoneNumber);

    /// <summary>
    /// Verifies the SMS 2FA setup code entered by the user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user attempting to verify the code.</param>
    /// <param name="code">The verification code entered by the user.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the code is valid; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> VerifySetupCodeAsync(string userId, string code);
}
