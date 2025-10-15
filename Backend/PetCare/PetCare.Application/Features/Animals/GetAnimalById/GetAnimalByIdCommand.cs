namespace PetCare.Application.Features.Animals.GetAnimalById;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to retrieve an animal by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the animal to retrieve.</param>
public sealed record GetAnimalByIdCommand(
    Guid Id)
    : IRequest<AnimalDto?>;
