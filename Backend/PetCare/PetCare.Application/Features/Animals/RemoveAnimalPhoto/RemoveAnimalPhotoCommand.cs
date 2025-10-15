namespace PetCare.Application.Features.Animals.RemoveAnimalPhoto;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to remove a specific photo from an animal's profile.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal whose photo is to be removed.</param>
/// <param name="PhotoUrl">The URL of the photo to remove from the animal's profile. Cannot be null or empty.</param>
public record RemoveAnimalPhotoCommand(
    Guid AnimalId,
    string PhotoUrl)
    : IRequest<AnimalDto>;
