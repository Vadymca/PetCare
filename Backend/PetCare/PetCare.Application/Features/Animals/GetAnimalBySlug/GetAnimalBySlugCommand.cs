namespace PetCare.Application.Features.Animals.GetAnimalBySlug;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to retrieve an animal by its unique slug identifier.
/// </summary>
/// <param name="Slug">The unique slug that identifies the animal to retrieve. Cannot be null or empty.</param>
public sealed record GetAnimalBySlugCommand(
    string Slug)
    : IRequest<AnimalDto>;
