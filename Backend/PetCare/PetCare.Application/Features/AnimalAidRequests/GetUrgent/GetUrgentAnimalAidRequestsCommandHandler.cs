namespace PetCare.Application.Features.AnimalAidRequests.GetUrgent;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles retrieving all urgent animal aid requests including donation summaries.
/// </summary>
public sealed class GetUrgentAnimalAidRequestsCommandHandler
    : IRequestHandler<GetUrgentAnimalAidRequestsCommand, List<UrgentAnimalAidRequestDto>>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUrgentAnimalAidRequestsCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">Service used to access animal aid request data.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="animalAidRequestService"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public GetUrgentAnimalAidRequestsCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService
            ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <summary>
    /// Handles the retrieval of urgent animal aid requests.
    /// </summary>
    /// <param name="request">The query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of urgent aid requests with donation statistics.</returns>
    public async Task<List<UrgentAnimalAidRequestDto>> Handle(
        GetUrgentAnimalAidRequestsCommand request,
        CancellationToken cancellationToken)
    {
        var urgentRequests = await this.animalAidRequestService
            .GetUrgentAnimalAidRequestsAsync(cancellationToken)
            .ConfigureAwait(false);

        var result = new List<UrgentAnimalAidRequestDto>(urgentRequests.Count);

        foreach (var aidRequest in urgentRequests)
        {
            var collectedAmount = await this.animalAidRequestService.GetCollectedAmountAsync(
                aidRequest.Id, cancellationToken);

            var donationsDto = aidRequest.Donations
                .Where(d => d.Donation != null)
                .Select(d => new DonationListDto(
                    d.Donation!.Id,
                    d.Donation.User?.FirstName,
                    d.Donation.User?.ProfilePhoto,
                    d.Donation.Amount,
                    d.Donation.Currency,
                    d.Donation.DonationDate,
                    d.Donation.Anonymous,
                    d.Donation.Purpose))
                .ToList()
                .AsReadOnly();

            var dto = new UrgentAnimalAidRequestDto(
                aidRequest.Id,
                aidRequest.Title.Value,
                aidRequest.EstimatedCost ?? 0m,
                collectedAmount,
                donationsDto.Count,
                donationsDto);

            result.Add(dto);
        }

        return result;
    }
}
