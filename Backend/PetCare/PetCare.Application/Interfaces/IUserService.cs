namespace PetCare.Application.Interfaces;

using PetCare.Domain.Aggregates;

/// <summary>
/// Service for user management operations.
/// Wraps UserManager to provide domain-specific functionality.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new user with the specified details.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <param name="firstName">User first name.</param>
    /// <param name="lastName">User last name.</param>
    /// <param name="phoneNumber">User phone number.</param>
    /// <param name="postalCode">User postal code.</param>
    /// <returns>The created user.</returns>
    Task<User> CreateUserAsync(string email, string password, string firstName, string lastName, string phoneNumber, string? postalCode);

    /// <summary>
    /// Finds a user by email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <returns>The user if found, null otherwise.</returns>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// Finds a user by ID.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>The user if found, null otherwise.</returns>
    Task<User?> FindByIdAsync(Guid userId);

    /// <summary>
    /// Generates an email confirmation token for the user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The confirmation token.</returns>
    Task<string> GenerateEmailConfirmationTokenAsync(User user);

    /// <summary>
    /// Confirms the user's email with the provided token.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="token">The confirmation token.</param>
    /// <returns>True if confirmation was successful, false otherwise.</returns>
    Task<bool> ConfirmEmailAsync(User user, string token);

    /// <summary>
    /// Checks if the provided password is correct for the user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password to check.</param>
    /// <returns>True if password is correct, false otherwise.</returns>
    Task<bool> CheckPasswordAsync(User user, string password);

    /// <summary>
    /// Gets the roles assigned to the user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>List of role names.</returns>
    Task<IList<string>> GetRolesAsync(User user);

    /// <summary>
    /// Resets the user's password using the provided reset token and new password.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="token">The reset token previously generated for the user.</param>
    /// <param name="newPassword">The new password to set.</param>
    /// <returns>True if the password was successfully reset; otherwise, false.</returns>
    Task<bool> ResetPasswordAsync(User user, string token, string newPassword);

    /// <summary>
    /// Generates a password reset token for the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The password reset token as a string.</returns>
    Task<string> GeneratePasswordResetTokenAsync(User user);

    Task<User?> GetCurrentUserAsync();

    Task<string> GetEmailAsync(User user);

    Task<string?> GetAuthenticatorKeyAsync(User user);

    Task<string> ResetAuthenticatorKeyAsync(User user);

    Task<string[]> GenerateNewTwoFactorRecoveryCodesAsync(User user, int count);
}
