namespace PetCare.Infrastructure.Services;

using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Options;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Options;

/// <summary>
/// Service for handling Facebook authentication.
/// </summary>
public sealed class FacebookAuthService : IFacebookAuthService
{
    private readonly FacebookSettings settings;
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="FacebookAuthService"/> class.
    /// </summary>
    /// <param name="options">The Facebook settings options.</param>
    /// <param name="httpClient">The HTTP client for making requests.</param>
    public FacebookAuthService(
        IOptions<FacebookSettings> options,
        HttpClient httpClient)
    {
        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Generates the Facebook login URL with the specified state parameter.
    /// </summary>
    /// <param name="state">The state parameter to include in the URL for CSRF protection.</param>
    /// <returns>The complete Facebook login URL.</returns>
    public string GetLoginUrl(string state)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["client_id"] = this.settings.AppId;
        query["redirect_uri"] = this.settings.RedirectUri;
        query["state"] = state;
        query["scope"] = "email,public_profile";

        return $"{this.settings.BaseUrl}?{query}";
    }

    /// <summary>
    /// Gets the access token from Facebook using the provided authorization code.
    /// </summary>
    /// <param name="code">The authorization code received from Facebook after user login.</param>
    /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
    /// <returns>The access token as a string.</returns>
    public async Task<string> GetAccessTokenAsync(string code, string redirectUri)
    {
        var tokenUrl = $"https://graph.facebook.com/v23.0/oauth/access_token?" +
                       $"client_id={this.settings.AppId}" +
                       $"&redirect_uri={HttpUtility.UrlEncode(redirectUri)}" +
                       $"&client_secret={this.settings.AppSecret}" +
                       $"&code={code}";

        var response = await this.httpClient.GetFromJsonAsync<JsonElement>(tokenUrl);
        if (response.TryGetProperty("access_token", out var token))
        {
            return token.GetString()!;
        }

        throw new InvalidOperationException("Не вдалося отримати access token від Facebook.");
    }

    /// <summary>
    /// Gets user information from Facebook using the provided access token.
    /// </summary>
    /// <param name="accessToken">The access token to authenticate the request.</param>
    /// <returns>A DTO containing user information.</returns>
    public async Task<FacebookUserInfoDto> GetUserInfoAsync(string accessToken)
    {
        var userInfoUrl = $"https://graph.facebook.com/me?fields=id,first_name,last_name,email,picture&access_token={accessToken}";
        var response = await this.httpClient.GetFromJsonAsync<JsonElement>(userInfoUrl);

        string? email = response.TryGetProperty("email", out var emailProp) ? emailProp.GetString() : null;
        string firstName = response.GetProperty("first_name").GetString()!;
        string lastName = response.GetProperty("last_name").GetString()!;
        string? profilePhoto = response.TryGetProperty("picture", out var pictureProp) ?
                                pictureProp.GetProperty("data").GetProperty("url").GetString() : null;

        return new FacebookUserInfoDto(
            Email: email!,
            FirstName: firstName,
            LastName: lastName,
            ProfilePhotoUrl: profilePhoto);
    }
}
