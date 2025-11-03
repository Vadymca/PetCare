namespace PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents the checkout information required by the frontend to render a LiqPay payment form.
/// </summary>
/// <param name="Data">The base64-encoded JSON payload to be sent to LiqPay.</param>
/// <param name="Signature">The base64 signature of the encoded data, used for verification by LiqPay.</param>
/// <param name="PublicKey">The LiqPay public key identifying the merchant account.</param>
/// <param name="GatewayUrl">The LiqPay gateway URL (usually https://www.liqpay.ua/api/3/checkout).</param>
/// <param name="OrderId">The unique internal order identifier used to track this payment.</param>
/// <param name="ResultUrl">The fully prepared result URL containing detailed context for the frontend.</param>
public sealed record LiqPayCheckoutResponseDto(
    string Data,
    string Signature,
    string PublicKey,
    string GatewayUrl,
    string OrderId,
    string ResultUrl);
