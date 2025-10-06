namespace PetCare.Application.Features.Animals.SubscribeToAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

public sealed record SubscribeToAnimalCommand(
    Guid AnimalId,
    Guid UserId)
    : IRequest<AnimalSubscriptionDto>;