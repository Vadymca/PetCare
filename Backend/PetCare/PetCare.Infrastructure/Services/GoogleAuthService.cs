namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Options;

/// <summary>
/// Service for handling Google OAuth authentication.
/// </summary>
public sealed class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleSettings settings;
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleAuthService"/> class.
    /// </summary>
    /// <param name="options">The Google OAuth configuration options.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to make HTTP requests to Google APIs.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> or <paramref name="httpClient"/> is null.</exception>
    public GoogleAuthService(
        IOptions<GoogleSettings> options,
        HttpClient httpClient)
    {
        this.settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Generates the Google OAuth login URL to redirect the user for authentication.
    /// </summary>
    /// <param name="state">A unique state value used to prevent CSRF attacks.</param>
    /// <returns>The full URL to redirect the user to Google's OAuth login page.</returns>
    public string GetLoginUrl(string state)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["client_id"] = this.settings.ClientId;
        query["redirect_uri"] = this.settings.RedirectUri;
        query["response_type"] = "code";
        query["scope"] = this.settings.Scope;
        query["state"] = state;
        query["access_type"] = "offline";
        query["prompt"] = "consent";

        return $"{this.settings.AuthorizationEndpoint}?{query}";
    }

    /// <summary>
    /// Exchanges an authorization code obtained from Google for an access token.
    /// </summary>
    /// <param name="code">The authorization code received from Google after user login.</param>
    /// <returns>A task representing the asynchronous operation, containing the access token as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the access token could not be obtained from Google.</exception>
    public async Task<string> GetAccessTokenAsync(string code)
    {
        var values = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = this.settings.ClientId,
            ["client_secret"] = this.settings.ClientSecret,
            ["redirect_uri"] = this.settings.RedirectUri,
            ["grant_type"] = "authorization_code",
        };

        var response = await this.httpClient.PostAsync(
            this.settings.TokenEndpoint,
            new FormUrlEncodedContent(values));

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (json.TryGetProperty("access_token", out var token))
        {
            return token.GetString()!;
        }

        throw new InvalidOperationException("Не вдалося отримати access token від Google.");
    }

    /// <summary>
    /// Retrieves the user's information from Google using the provided access token.
    /// </summary>
    /// <param name="accessToken">The OAuth 2.0 access token obtained from Google.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="GoogleUserInfoDto"/>
    /// with the user's email, first name, last name, and optionally profile photo URL.
    /// </returns>
    public async Task<GoogleUserInfoDto> GetUserInfoAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, this.settings.UserInfoEndpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await this.httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        string email = json.GetProperty("email").GetString()!;
        string firstName = json.GetProperty("given_name").GetString()!;
        string lastName = json.GetProperty("family_name").GetString()!;
        string? profilePhoto = json.TryGetProperty("picture", out var pic) ? pic.GetString() : null;

        return new GoogleUserInfoDto(email, firstName, lastName, profilePhoto);
    }
}
