namespace PetCare.Application.Features.Animals.GetFavoriteAnimals;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to retrieve the list of favorite animals for a specified user.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose favorite animals are to be retrieved.</param>
public sealed record GetFavoriteAnimalsCommand(Guid UserId)
    : IRequest<IReadOnlyList<AnimalListDto>>;
