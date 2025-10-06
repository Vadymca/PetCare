namespace PetCare.Application.Features.Animals.UnsubscribeFromAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using System;

public sealed record UnsubscribeFromAnimalCommand(
    Guid AnimalId,
    Guid UserId)
    : IRequest<UnsubscribeResultDto>;
