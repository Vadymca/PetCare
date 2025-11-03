namespace PetCare.Application.Dtos.Payments;

using System;
using PetCare.Domain.Enums;

/// <summary>Represents a user's recurring subscription.</summary>
public sealed record MySubscriptionDto(
    Guid Id,
    string Provider,
    decimal Amount,
    string Currency,
    SubscriptionStatus Status,
    SubscriptionScope ScopeType,
    Guid? ScopeId,
    DateTime CreatedAt,
    DateTime? NextChargeAt,
    DateTime? LastChargeAt);
