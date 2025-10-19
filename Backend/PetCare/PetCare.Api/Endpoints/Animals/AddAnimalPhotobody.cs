namespace PetCare.Api.Endpoints.Animals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Features.Animals.AddAnimalPhoto;

/// <summary>
/// Represents the data required to add a photo to an animal record.
/// </summary>
/// <param name="PhotoUrl">The URL of the photo to associate with the animal. Cannot be null or empty.</param>
public sealed record AddAnimalPhotobody(
    string PhotoUrl);
