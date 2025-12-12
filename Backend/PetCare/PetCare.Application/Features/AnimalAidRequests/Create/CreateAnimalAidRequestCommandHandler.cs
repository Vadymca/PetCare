namespace PetCare.Application.Features.AnimalAidRequests.Create;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
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
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAnimalAidRequestCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to manage animal aid requests.</param>
    /// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the dependencies is null.</exception>
    public CreateAnimalAidRequestCommandHandler(
        IAnimalAidRequestService animalAidRequestService,
        IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            contactPhone: request.ContactPhone,
            isUrgent: request.IsUrgent);

        var createdRequest = await this.animalAidRequestService.CreateAnimalAidRequestAsync(aidRequest, cancellationToken);

        return this.mapper.Map<AnimalAidRequestDetailsDto>(createdRequest);
    }
}
