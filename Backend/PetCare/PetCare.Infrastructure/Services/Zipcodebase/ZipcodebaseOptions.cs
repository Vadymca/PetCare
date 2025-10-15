namespace PetCare.Infrastructure.Services.Zipcodebase;

/// <summary>
/// Configuration options for Zipcodebase API.
/// </summary>
public sealed class ZipcodebaseOptions
{
    /// <summary>
    /// The name of the configuration section that contains settings for Zipcodebase integration.
    /// </summary>
    public const string SectionName = "Zipcodebase";

    /// <summary>
    /// Gets or sets the base URL of the Zipcodebase API.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for Zipcodebase.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country code for Zipcodebase API. Defaults to "UA" for Ukraine.
    /// </summary>
    public string Country { get; set; } = "UA";

    /// <summary>
    /// Gets or sets the language code (ISO 639-1) for localized names. Defaults to "uk" for Ukrainian.
    /// </summary>
    public string Language { get; set; } = "uk";
}
