namespace PetCare.Application.Features.Shelters.UpdateShelter;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Handles the update operation for a shelter by processing an <see cref="UpdateShelterCommand"/> and returning the
/// updated shelter data.
/// </summary>
/// <remarks>This handler coordinates the retrieval, update, and mapping of shelter information. It is typically
/// used within a MediatR pipeline to process shelter update requests. The handler requires valid service and mapping
/// dependencies to function correctly.</remarks>
public sealed class UpdateShelterCommandHandler : IRequestHandler<UpdateShelterCommand, ShelterDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateShelterCommandHandler"/> class with the specified shelter service and.
    /// object mapper.
    /// </summary>
    /// <param name="shelterService">The service used to perform shelter-related operations.</param>
    /// <param name="mapper">The mapper used to convert between domain models and data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if shelterService or mapper is null.</exception>
    public UpdateShelterCommandHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Updates the details of an existing shelter based on the specified update command.
    /// </summary>
    /// <param name="request">An <see cref="UpdateShelterCommand"/> containing the updated shelter information. The <c>Id</c> property
    /// specifies which shelter to update. Other properties provide the new values to apply.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
    /// <returns>A <see cref="ShelterDto"/> representing the updated shelter.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a shelter with the specified <c>Id</c> does not exist.</exception>
    public async Task<ShelterDto> Handle(UpdateShelterCommand request, CancellationToken cancellationToken)
    {
        var shelter = await this.shelterService.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{request.Id}' не знайдено.");

        Coordinates? coordinates = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
        {
            coordinates = Coordinates.From(request.Latitude.Value, request.Longitude.Value);
        }

        shelter.Update(
            name: request.Name,
            address: request.Address,
            coordinates: coordinates,
            contactPhone: request.ContactPhone,
            contactEmail: request.ContactEmail,
            description: request.Description,
            capacity: request.Capacity,
            photos: request.Photos,
            virtualTourUrl: request.VirtualTourUrl,
            workingHours: request.WorkingHours,
            socialMedia: request.SocialMedia);

        await this.shelterService.UpdateAsync(shelter, cancellationToken);

        return this.mapper.Map<ShelterDto>(shelter);
    }
}
