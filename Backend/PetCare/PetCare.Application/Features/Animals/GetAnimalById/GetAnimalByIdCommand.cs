namespace PetCare.Application.Features.Animals.GetAnimalById;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

public sealed record GetAnimalByIdCommand(
    Guid Id)
    : IRequest<AnimalDto?>;
