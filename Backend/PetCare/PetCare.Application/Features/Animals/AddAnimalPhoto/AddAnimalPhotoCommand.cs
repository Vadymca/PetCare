namespace PetCare.Application.Features.Animals.AddAnimalPhoto;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to add a photo to an existing animal record.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to which the photo will be added.</param>
/// <param name="PhotoUrl">The URL of the photo to associate with the animal. Cannot be null or empty.</param>
public record AddAnimalPhotoCommand(
    Guid AnimalId,
    string PhotoUrl)
    : IRequest<AnimalDto>;
