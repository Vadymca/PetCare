namespace PetCare.Application.Features.Payments.Subscriptions.ResetSubscription;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a command to reset a user's subscription, specifying the previous subscription, amount, currency, and
/// scope information.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose subscription is being reset.</param>
/// <param name="OldSubscriptionId">The unique identifier of the subscription to be replaced or reset.</param>
/// <param name="Amount">The amount to be applied to the new subscription. Must be a non-negative value.</param>
/// <param name="Currency">The ISO currency code representing the currency of the amount. Cannot be null or empty.</param>
/// <param name="Scope">The scope or context in which the subscription reset is performed. Typically indicates the type or domain of the
/// subscription.</param>
/// <param name="ScopeId">The unique identifier of the scope, if applicable. Can be null if the scope does not require an identifier.</param>
public sealed record ResetSubscriptionCommand(
    Guid UserId,
    Guid OldSubscriptionId,
    decimal Amount,
    string Currency,
    string Scope,
    Guid? ScopeId) : IRequest<SubscriptionDto>;
