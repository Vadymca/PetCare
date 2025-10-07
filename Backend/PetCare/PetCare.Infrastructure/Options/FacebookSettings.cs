namespace PetCare.Infrastructure.Options;

/// <summary>
/// Settings for Facebook OAuth integration.
/// </summary>
public sealed class FacebookSettings
{
    /// <summary>
    /// Gets or sets the Application ID provided by Facebook for OAuth authentication.
    /// </summary>
    public string AppId { get; set; } = null!;

    /// <summary>
    /// Gets or sets application secret key provided by Facebook for OAuth authentication.
    /// </summary>
    public string AppSecret { get; set; } = null!;

    /// <summary>
    /// Gets or sets the redirect URI where Facebook will send users after they have authenticated.
    /// </summary>
    public string RedirectUri { get; set; } = null!;

    /// <summary>
    /// Gets or sets the base URL for Facebook's OAuth endpoints.
    /// </summary>
    public string BaseUrl { get; set; } = null!;
}
