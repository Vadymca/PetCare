namespace PetCare.Application.Features.Animals.GetAnimalBySlug;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

public sealed record GetAnimalBySlugCommand(
    string Slug)
    : IRequest<AnimalDto>;
