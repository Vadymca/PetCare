namespace PetCare.Api.Endpoints.Payments.LiqPay;

using PetCare.Domain.Enums;

/// <summary>
/// Represents the client-facing request body for creating a LiqPay checkout session.
/// </summary>
public sealed record CreateLiqPayCheckoutRequest(
    decimal Amount,
    string Currency = "UAH",
    string? Description = null,
    bool IsRecurring = false,
    SubscriptionScope? Scope = null,
    Guid? EntityId = null,
    string? PayerName = null,
    string? PayerPhone = null,
    string? PayerEmail = null);
