namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Service for Google OAuth authentication.
/// </summary>
public interface IGoogleAuthService
{
    /// <summary>
    /// Generates Google login URL for redirect.
    /// </summary>
    /// <param name="state">The state parameter to include in the login URL for CSRF protection.</param>
    /// <returns>The generated Google login URL.</returns>
    string GetLoginUrl(string state);

    /// <summary>
    /// Exchanges authorization code for access token.
    /// </summary>
    /// <param name="code">The authorization code received from Google after user login.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<string> GetAccessTokenAsync(string code);

    /// <summary>
    /// Gets user info from Google using access token.
    /// </summary>
    /// <param name="accessToken">The OAuth 2.0 access token obtained from Google after successful authentication.</param>
    /// <returns>
    /// A <see cref="Task{GoogleUserInfoDto}"/> representing the asynchronous operation,
    /// containing the user's information retrieved from Google, such as email, name, and profile picture.
    /// </returns>
    Task<GoogleUserInfoDto> GetUserInfoAsync(string accessToken);
}
