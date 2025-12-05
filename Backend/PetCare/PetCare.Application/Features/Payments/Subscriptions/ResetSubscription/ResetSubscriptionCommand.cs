namespace PetCare.Application.Features.Payments.Subscriptions.ResetSubscription;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a command to reset a user's subscription, specifying the previous subscription, amount, currency, and
/// scope information.
/// </summary>
public sealed record ResetSubscriptionCommand(Guid SubscriptionId, Guid UserId)
    : IRequest<LiqPayCheckoutResponseDto>;
