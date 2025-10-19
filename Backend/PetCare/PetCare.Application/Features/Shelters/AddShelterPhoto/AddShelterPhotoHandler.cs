namespace PetCare.Application.Features.Shelters.AddShelterPhoto;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles adding a photo to a shelter.
/// </summary>
public sealed class AddShelterPhotoHandler : IRequestHandler<AddShelterPhotoCommand, ShelterDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddShelterPhotoHandler"/> class with the specified shelter service and object.
    /// mapper.
    /// </summary>
    /// <param name="shelterService">The service used to manage shelter-related operations. Cannot be null.</param>
    /// <param name="mapper">The object mapper used to convert between domain and data transfer objects. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if shelterService or mapper is null.</exception>
    public AddShelterPhotoHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ShelterDto> Handle(AddShelterPhotoCommand request, CancellationToken cancellationToken)
    {
        await this.shelterService.AddPhotoAsync(request.ShelterId, request.PhotoUrl, cancellationToken);

        var updatedShelter = await this.shelterService.GetByIdAsync(request.ShelterId, cancellationToken);

        return this.mapper.Map<ShelterDto>(updatedShelter!);
    }
}
