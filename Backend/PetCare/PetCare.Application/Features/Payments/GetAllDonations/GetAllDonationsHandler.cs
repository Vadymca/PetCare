namespace PetCare.Application.Features.Payments.GetAllDonations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve a list of all donations, returning donation details suitable for display or processing.
/// </summary>
/// <remarks>This handler uses the provided payment service to obtain donation data and maps it to a read-only
/// list of donation DTOs. The returned list includes information about each donation, such as donor identity (with
/// support for anonymous donations), amount, currency, and donation date. This class is typically used in scenarios
/// where an application needs to present or process all recorded donations.</remarks>
public sealed class GetAllDonationsHandler : IRequestHandler<GetAllDonationsCommand, IReadOnlyList<DonationListDto>>
{
    private readonly IPaymentService payments;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllDonationsHandler"/> class using the specified payment service.
    /// </summary>
    /// <param name="payments">The payment service implementation used to retrieve donation information. Cannot be null.</param>
    public GetAllDonationsHandler(IPaymentService payments)
        => this.payments = payments;

    /// <summary>
    /// Retrieves a read-only list of donation records based on the specified command.
    /// </summary>
    /// <param name="request">The command containing criteria for retrieving all donations.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of donation data transfer objects representing the retrieved donations.</returns>
    public async Task<IReadOnlyList<DonationListDto>> Handle(GetAllDonationsCommand request, CancellationToken ct)
    {
        var donations = await this.payments.ListAllDonationsAsync(ct);

        return donations.Select(d => new DonationListDto(
            d.Id,
            d.Anonymous ? "Anonymous" : d.User?.FirstName ?? "Anonymous",
            d.Anonymous ? null : d.User?.ProfilePhoto,
            d.Amount,
            d.Currency,
            d.DonationDate,
            d.Anonymous)).ToList();
    }
}
