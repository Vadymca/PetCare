namespace PetCare.Application.Features.Payments.GetMyUpcomingPayments;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve a list of upcoming payments for the current user.
/// </summary>
/// <remarks>This handler processes the command to obtain expected payments by interacting with the subscription
/// service. It returns payment details for all subscriptions with scheduled charges. The returned list may be empty if
/// no upcoming payments are found.</remarks>
public sealed class GetMyUpcomingPaymentsHandler : IRequestHandler<GetMyUpcomingPaymentsCommand, IReadOnlyList<MyUpcomingPaymentDto>>
{
    private readonly ISubscriptionService subscriptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyUpcomingPaymentsHandler"/> class using the specified subscription service.
    /// </summary>
    /// <param name="subscriptions">The subscription service used to retrieve subscription-related data. Cannot be null.</param>
    public GetMyUpcomingPaymentsHandler(ISubscriptionService subscriptions) => this.subscriptions = subscriptions;

    /// <summary>
    /// Retrieves a read-only list of upcoming payments for the current user based on the specified command.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch expected payments and maps them to
    /// data transfer objects for client consumption.</remarks>
    /// <param name="request">The command containing the user identifier for whom upcoming payments are to be retrieved.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of data transfer objects representing the user's upcoming payments. The list will be empty if
    /// no upcoming payments are found.</returns>
    public async Task<IReadOnlyList<MyUpcomingPaymentDto>> Handle(GetMyUpcomingPaymentsCommand request, CancellationToken ct)
    {
        var items = await this.subscriptions.GetMyExpectedPaymentsAsync(request.UserId, ct);
        return items.Select(i => new MyUpcomingPaymentDto(
            i.Subscription.Id,
            i.Subscription.Provider,
            i.Subscription.Amount,
            i.Subscription.Currency,
            i.NextChargeAt)).ToList();
    }
}
