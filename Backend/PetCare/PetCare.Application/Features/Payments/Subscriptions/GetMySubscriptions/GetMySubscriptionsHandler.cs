namespace PetCare.Application.Features.Payments.Subscriptions.GetMySubscriptions;

using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Handles requests to retrieve the list of recurring subscriptions for a specific user.
/// </summary>
/// <remarks>This handler processes a GetMySubscriptionsCommand and returns a read-only list of MySubscriptionDto
/// objects representing the user's active and historical recurring subscriptions. The returned list may be empty if the
/// user has no subscriptions. This class is typically used in a CQRS pattern to separate query logic from command
/// logic.</remarks>
public sealed class GetMySubscriptionsHandler : IRequestHandler<GetMySubscriptionsCommand, IReadOnlyList<MySubscriptionDto>>
{
    private readonly ISubscriptionService subscriptions;
    private readonly IGuardianshipService guardianships;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMySubscriptionsHandler"/> class using the specified subscription service.
    /// </summary>
    /// <param name="subscriptions">The subscription service used to retrieve subscription information. Cannot be null.</param>
    /// <param name="guardianships">The guardianship service used for additional user context. Cannot be null.</param>
    public GetMySubscriptionsHandler(
        ISubscriptionService subscriptions,
        IGuardianshipService guardianships)
    {
        this.subscriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions));
        this.guardianships = guardianships ?? throw new ArgumentNullException(nameof(guardianships));
    }

    /// <summary>
    /// Retrieves a read-only list of subscription details for the specified user.
    /// </summary>
    /// <param name="request">The command containing the user identifier for which to retrieve subscriptions.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A read-only list of subscription data transfer objects representing the user's recurring subscriptions. The list
    /// will be empty if the user has no subscriptions.</returns>
    public async Task<IReadOnlyList<MySubscriptionDto>> Handle(GetMySubscriptionsCommand request, CancellationToken ct)
    {
        var items = await this.subscriptions.GetMyRecurringAsync(request.UserId, ct);

        var result = new List<MySubscriptionDto>(items.Count);

        foreach (var s in items)
        {
            var purpose = await this.BuildPurposeAsync(s, ct);

            result.Add(new MySubscriptionDto(
                s.Id,
                s.Provider,
                s.Amount,
                s.Currency,
                s.Status,
                s.ScopeType,
                s.ScopeId,
                s.ProviderSubscriptionId,
                s.CreatedAt,
                s.NextChargeAt,
                s.LastChargeAt,
                purpose));
        }

        return result;
    }

    private async Task<string> BuildPurposeAsync(PaymentSubscription subscription, CancellationToken ct)
    {
        if (subscription.ScopeType == SubscriptionScope.Guardianship && subscription.ScopeId is Guid gId)
        {
            var g = await this.guardianships.GetByIdAsync(gId, ct);

            if (g?.Animal is null)
            {
                return "Опіка тварини";
            }

            return $"Опіка тварини: {g.Animal.Name}";
        }

        return "Підписка";
    }
}
