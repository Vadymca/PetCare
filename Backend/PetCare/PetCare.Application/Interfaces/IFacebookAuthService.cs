namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Interface for Facebook authentication service.
/// </summary>
public interface IFacebookAuthService
{
    /// <summary>
    /// Generates the Facebook login URL with the specified state parameter.
    /// </summary>
    /// <param name="state">The state parameter to include in the login URL for CSRF protection.</param>
    /// <returns>The generated Facebook login URL.</returns>
    string GetLoginUrl(string state);

    /// <summary>
    /// Gets the access token from Facebook using the provided authorization code.
    /// </summary>
    /// <param name="code">The authorization code received from Facebook after user login.</param>
    /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
    /// <returns>The access token as a string.</returns>
    Task<string> GetAccessTokenAsync(string code, string redirectUri);

    /// <summary>
    /// Gets user information from Facebook using the provided access token.
    /// </summary>
    /// <param name="accessToken">The access token to authenticate the request.</param>
    /// <returns>A DTO containing user information.</returns>
    Task<FacebookUserInfoDto> GetUserInfoAsync(string accessToken);
}
