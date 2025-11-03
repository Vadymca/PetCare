namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>Represents an upcoming expected charge.</summary>
public sealed record MyUpcomingPaymentDto(
    Guid SubscriptionId,
    string Provider,
    decimal Amount,
    string Currency,
    DateTime? NextChargeAt);
