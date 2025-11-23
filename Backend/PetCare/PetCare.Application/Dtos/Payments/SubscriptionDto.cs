namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents a subscription record, including user, provider, billing, and status information.
/// </summary>
/// <param name="Id">The unique identifier for the subscription.</param>
/// <param name="UserId">The unique identifier of the user associated with the subscription.</param>
/// <param name="Amount">The recurring billing amount for the subscription.</param>
/// <param name="Currency">The ISO currency code for the subscription amount (for example, "USD").</param>
/// <param name="Provider">The name of the external service or payment provider managing the subscription.</param>
/// <param name="ProviderSubscriptionId">The identifier assigned to the subscription by the external provider.</param>
/// <param name="NextChargeAt">The date and time of the next scheduled charge, or null if not scheduled.</param>
/// <param name="Status">The current status of the subscription (for example, "Active", "Canceled").</param>
/// <param name="Scope">The logical scope or context to which the subscription applies (for example, "Organization", "Project").</param>
/// <param name="ScopeId">The unique identifier of the scope entity, or null if not applicable.</param>
public sealed record SubscriptionDto(
    Guid Id,
    Guid UserId,
    decimal Amount,
    string Currency,
    string Provider,
    string ProviderSubscriptionId,
    DateTime? NextChargeAt,
    string Status,
    string Scope,
    Guid? ScopeId);
