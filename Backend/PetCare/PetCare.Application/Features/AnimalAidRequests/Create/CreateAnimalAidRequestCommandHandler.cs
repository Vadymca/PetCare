namespace PetCare.Application.Features.AnimalAidRequests.Create;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Handles the creation of a new animal aid request using <see cref="CreateAnimalAidRequestCommand"/>.
/// </summary>
public sealed class CreateAnimalAidRequestCommandHandler
    : IRequestHandler<CreateAnimalAidRequestCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAnimalAidRequestCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to manage animal aid requests.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies is null.</exception>
    public CreateAnimalAidRequestCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto> Handle(
        CreateAnimalAidRequestCommand request,
        CancellationToken cancellationToken)
    {
        var aidRequest = AnimalAidRequest.Create(
            userId: null,
            shelterId: request.ShelterId,
            title: request.Title,
            shortDescription: request.ShortDescription,
            description: request.Description,
            category: Enum.Parse<AidCategory>(request.Category),
            status: request.Status,
            estimatedCost: request.EstimatedCost,
            photos: request.Photos,
            curatorFullName: request.CuratorFullName,
            contactPhone: request.ContactPhone,
            isUrgent: request.IsUrgent);

        var createdRequest = await this.animalAidRequestService.CreateAnimalAidRequestAsync(aidRequest, cancellationToken);

        var donatedAmount =
                await this.animalAidRequestService.GetCollectedAmountAsync(
                    aidRequest.Id,
                    cancellationToken);

        var donationsCount = await this.animalAidRequestService.GetDonationsCountAsync(createdRequest.Id, cancellationToken);

        var dto = new AnimalAidRequestDetailsDto(
            createdRequest.Id,
            createdRequest.Slug.Value,
            createdRequest.Shelter != null ? new ShelterInfoDto(createdRequest.Shelter.Id, createdRequest.Shelter.Name.Value, createdRequest.Shelter.Slug.Value) : null,
            createdRequest.Title.Value,
            createdRequest.Description ?? string.Empty,
            createdRequest.Category,
            createdRequest.EstimatedCost ?? 0m,
            donatedAmount,
            donationsCount,
            createdRequest.Status,
            createdRequest.Photos,
            createdRequest.CuratorFullName,
            createdRequest.ContactPhone?.Value,
            createdRequest.CreatedAt);

        return dto;
    }
}
