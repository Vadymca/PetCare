namespace PetCare.Application.Features.Animals.GetFavoriteAnimals;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using System.Collections.Generic;

public sealed record GetFavoriteAnimalsCommand(Guid UserId)
    : IRequest<IReadOnlyList<AnimalListDto>>;
