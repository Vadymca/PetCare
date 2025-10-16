namespace PetCare.Application.Features.Shelters.CreateShelter;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="CreateShelterCommand"/>.
/// </summary>
public sealed class CreateShelterCommandHandler : IRequestHandler<CreateShelterCommand, ShelterDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateShelterCommandHandler"/> class.
    /// </summary>
    /// <param name="shelterService">The shelter service instance.</param>
    /// <param name="mapper">The mapper instance.</param>
    public CreateShelterCommandHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<ShelterDto> Handle(CreateShelterCommand request, CancellationToken cancellationToken)
    {
        var coordinates = PetCare.Domain.ValueObjects.Coordinates.From(request.Latitude, request.Longitude);

        var addedShelter = await this.shelterService.CreateAsync(
             name: request.Name,
             address: request.Address,
             latitude: request.Latitude,
             longitude: request.Longitude,
             contactPhone: request.ContactPhone,
             contactEmail: request.ContactEmail,
             description: request.Description,
             capacity: request.Capacity,
             currentOccupancy: 0,
             photos: request.Photos,
             virtualTourUrl: request.VirtualTourUrl,
             workingHours: request.WorkingHours,
             socialMedia: request.SocialMedia,
             managerId: request.ManagerId,
             cancellationToken: cancellationToken);

        return this.mapper.Map<ShelterDto>(addedShelter);
    }
}
