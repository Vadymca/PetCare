namespace PetCare.Application.Features.Payments.CancelSubscription;

using MediatR;

/// <summary>
/// Represents a command to cancel an active payment subscription by its provider-assigned identifier.
/// </summary>
/// <param name="ProviderSubscriptionId">The provider-assigned subscription identifier.</param>
public sealed record CancelSubscriptionCommand(string ProviderSubscriptionId) : IRequest;
