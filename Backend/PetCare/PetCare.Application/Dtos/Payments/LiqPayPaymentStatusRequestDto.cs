namespace PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve the payment status for a specific order from LiqPay.
/// </summary>
/// <param name="OrderId">The unique identifier of the order for which the payment status is requested. Cannot be null or empty.</param>
public sealed record LiqPayPaymentStatusRequestDto(string OrderId);
