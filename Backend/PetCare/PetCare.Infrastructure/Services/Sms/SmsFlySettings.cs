namespace PetCare.Infrastructure.Services.Sms;

/// <summary>
/// Represents the configuration settings required to connect to the SmsFly SMS gateway API.
/// </summary>
/// <remarks>Use this class to provide authentication credentials and connection details when integrating with the
/// SmsFly service. All properties must be set with valid values before making API requests.</remarks>
public class SmsFlySettings
{
    /// <summary>
    /// Gets or sets the API key used to authenticate requests to external services.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the message sender.
    /// </summary>
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base URL used for API requests.
    /// </summary>
    public string BaseUrl { get; set; } = "https://sms-fly.ua/api/v2/api.php";
}
