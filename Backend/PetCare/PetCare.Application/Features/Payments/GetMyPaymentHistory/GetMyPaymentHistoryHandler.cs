namespace PetCare.Application.Features.Payments.GetMyPaymentHistory;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve the payment history for the current user.
/// </summary>
/// <remarks>This handler processes a GetMyPaymentHistoryCommand and returns a read-only list of payment history
/// records associated with the specified user. The returned list contains details for each payment, including amount,
/// currency, status, and related entities. This class is typically used in scenarios where a user needs to view their
/// own payment transactions. Thread safety depends on the underlying IPaymentService implementation.</remarks>
public sealed class GetMyPaymentHistoryHandler : IRequestHandler<GetMyPaymentHistoryCommand, IReadOnlyList<MyPaymentHistoryDto>>
{
    private readonly IPaymentService payments;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyPaymentHistoryHandler"/> class using the specified payment service.
    /// </summary>
    /// <param name="payments">The payment service used to retrieve payment history data. Cannot be null.</param>
    public GetMyPaymentHistoryHandler(IPaymentService payments) => this.payments = payments;

    /// <summary>
    /// Retrieves the payment history for the specified user and returns a read-only list of payment history records.
    /// </summary>
    /// <param name="request">The command containing the user identifier for whom the payment history is to be retrieved.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A read-only list of payment history data transfer objects representing the user's payment transactions. The list
    /// will be empty if no payment history is found for the user.</returns>
    public async Task<IReadOnlyList<MyPaymentHistoryDto>> Handle(GetMyPaymentHistoryCommand request, CancellationToken ct)
    {
        var items = await this.payments.GetMyPaymentsAsync(request.UserId, ct);
        return items.Select(p => new MyPaymentHistoryDto(
            p.Id,
            p.Amount,
            p.Currency,
            p.Status.ToString(),
            p.Recurring,
            p.Purpose ?? string.Empty,
            p.TargetEntity ?? string.Empty,
            p.TargetEntityId,
            p.DonationDate)).ToList();
    }
}
