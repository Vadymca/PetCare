namespace PetCare.Infrastructure.Options;

/// <summary>
/// Twilio SMS provider settings.
/// </summary>
public sealed class TwilioSettings
{
    /// <summary>
    /// Gets twilio account SID.
    /// </summary>
    public string AccountSid { get; init; } = string.Empty;

    /// <summary>
    /// Gets twilio authentication token.
    /// </summary>
    public string AuthToken { get; init; } = string.Empty;

    /// <summary>
    /// Gets sender phone number registered in Twilio.
    /// </summary>
    public string FromPhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether enables detailed logging for SMS operations.
    /// </summary>
    public bool EnableLogging { get; init; }

    /// <summary>
    /// Gets timeout for SMS requests in seconds.
    /// </summary>
    public int TimeoutSeconds { get; init; }
}
