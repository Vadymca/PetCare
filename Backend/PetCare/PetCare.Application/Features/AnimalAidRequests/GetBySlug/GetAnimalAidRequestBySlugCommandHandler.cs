namespace PetCare.Application.Features.AnimalAidRequests.GetBySlug;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAnimalAidRequestBySlugCommand"/> requests.
/// </summary>
public sealed class GetAnimalAidRequestBySlugCommandHandler : IRequestHandler<GetAnimalAidRequestBySlugCommand, AnimalAidRequestDetailsDto?>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalAidRequestBySlugCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to retrieve animal aid request data.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the dependencies are null.</exception>
    public GetAnimalAidRequestBySlugCommandHandler(IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto?> Handle(GetAnimalAidRequestBySlugCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(request.Slug));
        }

        var aidRequest = await this.animalAidRequestService.GetAnimalAidRequestBySlugAsync(request.Slug, cancellationToken)
            ?? throw new InvalidOperationException($"Запит на допомогу зі slug '{request.Slug}' не знайдено.");

        var donatedAmount = await this.animalAidRequestService.GetCollectedAmountAsync(aidRequest.Id, cancellationToken);

        var dto = new AnimalAidRequestDetailsDto(
            aidRequest.Id,
            aidRequest.Slug.Value,
            aidRequest.Shelter != null ? new ShelterInfoDto(aidRequest.Shelter.Id, aidRequest.Shelter.Name.Value, aidRequest.Shelter.Slug.Value) : null,
            aidRequest.Title.Value,
            aidRequest.Description ?? string.Empty,
            aidRequest.Category,
            aidRequest.EstimatedCost ?? 0m,
            donatedAmount,
            aidRequest.Status,
            aidRequest.Photos,
            aidRequest.CreatedAt);

        return dto;
    }
}
