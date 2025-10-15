namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.AuthDtos;
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
    /// Creates a new user from Facebook user info.
    /// </summary>
    /// <param name="fbUser">Facebook user info DTO.</param>
    /// <returns>The created user.</returns>
    Task<User> CreateUserFromFacebookAsync(FacebookUserInfoDto fbUser);

    /// <summary>
    /// Creates a new user based on Google user information without a password.
    /// </summary>
    /// <param name="googleUser">The Google user information.</param>
    /// <returns>The created <see cref="User"/> entity.</returns>
    Task<User> CreateUserFromGoogleAsync(GoogleUserInfoDto googleUser);

    /// <summary>
    /// Updates the last login date of a user.
    /// </summary>
    /// <param name="user">The user aggregate.</param>
    /// <param name="date">The date and time of the last login.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetLastLoginAsync(User user, DateTime date);

    /// <summary>
    /// Finds a user by email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <returns>The user if found, null otherwise.</returns>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// Finds a user by their phone number, if one exists.
    /// </summary>
    /// <param name="phone">The phone number to search for.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the <see cref="User"/>
    /// if found, or <c>null</c> if no user is associated with the given phone number.
    /// </returns>
    Task<User?> FindByPhoneAsync(string phone);

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

    /// <summary>
    /// Gets the currently authenticated user from the HTTP context.
    /// </summary>
    /// <returns>The current user if authenticated, null otherwise.</returns>
    Task<User?> GetCurrentUserAsync();

    /// <summary>
    /// Gets the email address of the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The user's email address.</returns>
    Task<string> GetEmailAsync(User user);

    /// <summary>
    /// Gets the authenticator key for TOTP-based two-factor authentication.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The authenticator key, or null if none exists.</returns>
    Task<string?> GetAuthenticatorKeyAsync(User user);

    /// <summary>
    /// Resets the authenticator key for TOTP-based two-factor authentication.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The newly generated authenticator key.</returns>
    Task<string> ResetAuthenticatorKeyAsync(User user);

    /// <summary>
    /// Generates new recovery codes for TOTP-based two-factor authentication.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="count">The number of recovery codes to generate.</param>
    /// <returns>An array of generated recovery codes.</returns>
    Task<string[]> GenerateNewTwoFactorRecoveryCodesAsync(User user, int count);

    /// <summary>
    /// Verifies a TOTP (Time-based One-Time Password) code for the specified user.
    /// </summary>
    /// <param name="user">The user whose TOTP code is being verified.</param>
    /// <param name="code">The TOTP code provided by the user.</param>
    /// <returns>
    /// <c>true</c> if the code is valid for the current TOTP window; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> VerifyTotpCodeAsync(User user, string code);

    /// <summary>
    /// Enables two-factor authentication for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to enable two-factor authentication.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EnableTwoFactorAsync(User user);

    /// <summary>
    /// Disables TOTP two-factor authentication for the specified user.
    /// </summary>
    /// <param name="user">The user whose TOTP is being disabled.</param>
    /// <returns><c>true</c> if TOTP was disabled successfully; otherwise, <c>false</c>.</returns>
    Task<bool> DisableTotpAsync(User user);

    /// <summary>
    /// Retrieves the backup codes for the specified user.
    /// </summary>
    /// <param name="user">The user whose backup codes are requested.</param>
    /// <returns>A list of backup codes.</returns>
    Task<IReadOnlyList<string>> GetTotpBackupCodesAsync(User user);

    /// <summary>
    /// Regenerates new TOTP backup codes for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to regenerate backup codes.</param>
    /// <returns>A read-only list of newly generated codes.</returns>
    Task<IReadOnlyList<string>> RegenerateTotpBackupCodesAsync(User user);

    /// <summary>
    /// Verifies a TOTP backup code for the specified user.
    /// </summary>
    /// <param name="user">The user whose backup code is being verified.</param>
    /// <param name="code">The backup code provided by the user.</param>
    /// <returns>
    /// <c>true</c> if the code is valid; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> VerifyTotpBackupCodeAsync(User user, string code);

    /// <summary>
    /// Marks the user's phone number as confirmed.
    /// </summary>
    /// <param name="user">The user whose phone number will be confirmed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConfirmPhoneNumberAsync(User user);

    /// <summary>
    /// Disables SMS 2FA for the user.
    /// </summary>
    /// <param name="user">The user to disable SMS 2FA for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DisableSms2FaAsync(User user);

    /// <summary>
    /// Retrieves the current two-factor authentication (2FA) status for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to get the 2FA status.</param>
    /// <returns>
    /// A <see cref="TwoFactorStatusResponseDto"/> containing the status of each 2FA method:
    /// <list type="bullet">
    /// <item><description><c>IsTwoFactorEnabled</c> — overall 2FA enabled flag (e.g., TOTP or other methods).</description></item>
    /// <item><description><c>IsSms2FaEnabled</c> — SMS 2FA enabled flag.</description></item>
    /// </list>
    /// </returns>
    TwoFactorStatusResponseDto GetTwoFactorStatus(User user);

    /// <summary>
    /// Disables all 2FA methods for the specified user.
    /// </summary>
    /// <param name="user">The user whose 2FA methods should be disabled.</param>
    /// <returns>A task that represents the asynchronous operation. Returns true if successful.</returns>
    Task<bool> DisableAllTwoFactorAsync(User user);

    /// <summary>
    /// Redeems a two-factor authentication recovery code.
    /// </summary>
    /// <param name="user">The user who is redeeming the recovery code.</param>
    /// <param name="code">The recovery code.</param>
    /// <returns><c>true</c> if the code was valid and successfully redeemed; otherwise <c>false</c>.</returns>
    Task<bool> RedeemRecoveryCodeAsync(User user, string code);

    /// <summary>
    /// Replaces all current roles of the specified user with a new role.
    /// </summary>
    /// <param name="user">The user entity whose roles will be replaced.</param>
    /// <param name="newRole">The new role to assign to the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReplaceRoleAsync(User user, string newRole);

    /// <summary>
    /// Changes the password of the user with the specified Id by generating a reset token
    /// and applying the new password using <see cref="UserManager{User}"/>.
    /// Throws an exception if the password change fails.
    /// </summary>
    /// <param name="userId">The Id of the user whose password will be changed.</param>
    /// <param name="newPassword">The new plain text password to set.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the password reset operation fails.</exception>
    Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken);

    /// <summary>
    /// Saves a 2FA token for the specified user with a time-to-live.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">The 2FA token to save.</param>
    /// <param name="ttl">The time-to-live for the token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Save2FaTokenAsync(Guid userId, string token, TimeSpan ttl);

    /// <summary>
    /// Verifies a 2FA token for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">The token to verify.</param>
    /// <returns>True if valid, otherwise false.</returns>
    Task<bool> Verify2FaTokenAsync(Guid userId, string token);

    /// <summary>
    /// Gets a user by the temporary 2FA token.
    /// Returns null if token is invalid or expired.
    /// </summary>
    /// <param name="twoFaToken">The temporary 2FA token.</param>
    /// <returns>The <see cref="User"/> or null.</returns>
    Task<User?> GetUserByTwoFaTokenAsync(string twoFaToken);
}
