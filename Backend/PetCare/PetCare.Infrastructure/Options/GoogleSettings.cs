namespace PetCare.Infrastructure.Options;

/// <summary>
/// Configuration for Google OAuth.
/// </summary>
public sealed class GoogleSettings
{
    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    public string ClientSecret { get; set; } = default!;

    /// <summary>
    /// Gets or sets the redirect URI.
    /// </summary>
    public string RedirectUri { get; set; } = default!;

    /// <summary>
    /// Gets or sets the authorization endpoint.
    /// </summary>
    public string AuthorizationEndpoint { get; set; } = default!;

    /// <summary>
    /// Gets or sets the token endpoint.
    /// </summary>
    public string TokenEndpoint { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user info endpoint.
    /// </summary>
    public string UserInfoEndpoint { get; set; } = default!;

    /// <summary>
    /// Gets or sets the scope for OAuth.
    /// </summary>
    public string Scope { get; set; } = default!;
}
