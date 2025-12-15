namespace PetCare.Application.Features.AnimalAidRequests.GetAllAnimalAidRequests;

using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAllAnimalAidRequestsCommand"/> requests.
/// </summary>
public sealed class GetAllAnimalAidRequestsCommandHandler
    : IRequestHandler<GetAllAnimalAidRequestsCommand, IReadOnlyList<AnimalAidRequestListDto>>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllAnimalAidRequestsCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">Service used to access animal aid request data.</param>
    public GetAllAnimalAidRequestsCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AnimalAidRequestListDto>> Handle(
     GetAllAnimalAidRequestsCommand request,
     CancellationToken cancellationToken)
    {
        var requests = await this.animalAidRequestService.GetAllAnimalAidRequestsAsync(cancellationToken);

        var result = new List<AnimalAidRequestListDto>(requests.Count);

        foreach (var aidRequest in requests)
        {
            var donatedAmount =
                await this.animalAidRequestService.GetCollectedAmountAsync(
                    aidRequest.Id,
                    cancellationToken);

            var dto = new AnimalAidRequestListDto(
                aidRequest.Id,
                aidRequest.Slug.Value,
                aidRequest.Shelter != null ? new ShelterInfoDto(aidRequest.Shelter.Id, aidRequest.Shelter.Name.Value, aidRequest.Shelter.Slug.Value) : null,
                aidRequest.Title.Value,
                aidRequest.ShortDescription ?? string.Empty,
                aidRequest.Category,
                donatedAmount,
                aidRequest.EstimatedCost ?? 0m,
                aidRequest.Status,
                aidRequest.Photos.FirstOrDefault());

            result.Add(dto);
        }

        return result;
    }
}
