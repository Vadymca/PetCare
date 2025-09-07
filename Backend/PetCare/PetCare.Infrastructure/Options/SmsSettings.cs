namespace PetCare.Infrastructure.Options;

/// <summary>
/// General SMS settings.
/// </summary>
public sealed class SmsSettings
{
    /// <summary>
    /// Gets sMS provider (e.g., Twilio).
    /// </summary>
    public string Provider { get; init; } = string.Empty;

    /// <summary>
    /// Gets application name to display in messages.
    /// </summary>
    public string ApplicationName { get; init; } = string.Empty;

    /// <summary>
    /// Gets code expiration time in minutes.
    /// </summary>
    public int CodeExpirationMinutes { get; init; }

    /// <summary>
    /// Gets maximum allowed attempts within the defined period.
    /// </summary>
    public int MaxAttemptsPerPeriod { get; init; }

    /// <summary>
    /// Gets period in minutes for rate limiting.
    /// </summary>
    public int RateLimitPeriodMinutes { get; init; }
}
