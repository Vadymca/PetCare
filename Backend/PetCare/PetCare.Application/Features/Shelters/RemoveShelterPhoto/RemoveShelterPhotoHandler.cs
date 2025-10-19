namespace PetCare.Application.Features.Shelters.RemoveShelterPhoto;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for removing a photo from a shelter.
/// </summary>
public sealed class RemoveShelterPhotoHandler : IRequestHandler<RemoveShelterPhotoCommand, ShelterDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveShelterPhotoHandler"/> class.
    /// </summary>
    /// <param name="shelterService">The service responsible for managing shelters.</param>
    /// <param name="mapper">The object mapper used for converting domain entities to DTOs.</param>
    public RemoveShelterPhotoHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ShelterDto> Handle(RemoveShelterPhotoCommand request, CancellationToken cancellationToken)
    {
        var removed = await this.shelterService.RemovePhotoAsync(request.ShelterId, request.PhotoUrl, cancellationToken);

        if (!removed)
        {
            throw new InvalidOperationException($"Фото не знайдено для притулку з Id '{request.ShelterId}'.");
        }

        var updatedShelter = await this.shelterService.GetByIdAsync(request.ShelterId, cancellationToken);

        return this.mapper.Map<ShelterDto>(updatedShelter!);
    }
}
