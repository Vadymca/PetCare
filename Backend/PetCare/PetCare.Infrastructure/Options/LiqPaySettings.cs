namespace PetCare.Infrastructure.Options;

/// <summary>
/// Represents the configuration settings required to integrate with the LiqPay payment service.
/// </summary>
/// <remarks>This class encapsulates the credentials and endpoint URLs necessary for authenticating and
/// interacting with the LiqPay API. It includes options for specifying sandbox mode and customizing API endpoints. All
/// properties must be set with valid values before initiating payment operations.</remarks>
public sealed class LiqPaySettings
{
    /// <summary>
    /// Gets or sets the public key used for cryptographic operations or identity verification.
    /// </summary>
    public string PublicKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets the private key used for authentication or encryption operations.
    /// </summary>
    public string PrivateKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the environment is configured to operate in sandbox mode.
    /// </summary>
    /// <remarks>When enabled, sandbox mode may restrict certain operations or provide a safe environment for
    /// testing and development. This property should be set to <see langword="true"/> when running in a non-production
    /// context to avoid unintended side effects.</remarks>
    public bool Sandbox { get; set; }

    /// <summary>
    /// Gets or sets the base URL for API requests to the LiqPay service.
    /// </summary>
    /// <remarks>Set this property to specify a custom API endpoint if required. The default value points to
    /// the official LiqPay API base URL.</remarks>
    public string ApiBase { get; set; } = "https://www.liqpay.ua/api";

    /// <summary>
    /// Gets or sets the URL where the result can be accessed.
    /// </summary>
    public string ResultUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the base URL of the server to which requests are sent.
    /// </summary>
    public string ServerUrl { get; set; } = null!;
}
