namespace PetCare.Application.Features.Payments.GetProjectDonations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve a list of donations for a specified project.
/// </summary>
/// <remarks>This handler processes the GetProjectDonationsCommand and returns donation details as a read-only
/// list of DonationListDto objects. It uses the provided IPaymentService to query donations associated with the given
/// project. This class is typically used in a CQRS pattern to separate query logic from command handling.</remarks>
public sealed class GetProjectDonationsHandler : IRequestHandler<GetProjectDonationsCommand, IReadOnlyList<DonationListDto>>
{
    private readonly IPaymentService payments;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProjectDonationsHandler"/> class using the specified payment service.
    /// </summary>
    /// <param name="payments">The payment service used to retrieve and process donation information for projects. Cannot be null.</param>
    public GetProjectDonationsHandler(IPaymentService payments)
        => this.payments = payments;

    /// <summary>
    /// Retrieves a read-only list of donations associated with the specified project.
    /// </summary>
    /// <remarks>The returned list includes both anonymous and non-anonymous donations. For anonymous
    /// donations, user information such as name and profile photo will be omitted.</remarks>
    /// <param name="request">The command containing the project identifier for which to retrieve donations. Must not be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A read-only list of donation data transfer objects representing donations for the specified project. The list
    /// will be empty if no donations are found.</returns>
    public async Task<IReadOnlyList<DonationListDto>> Handle(GetProjectDonationsCommand request, CancellationToken ct)
    {
        var donations = await this.payments.ListDonationsByProjectAsync(request.ProjectId, ct);

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
