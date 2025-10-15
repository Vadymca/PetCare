namespace PetCare.Application.Features.Animals.SubscribeToAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to subscribe a user to updates for a specific animal.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal to subscribe to.</param>
/// <param name="UserId">The unique identifier of the user who will be subscribed.</param>
public sealed record SubscribeToAnimalCommand(
    Guid AnimalId,
    Guid UserId)
    : IRequest<AnimalSubscriptionDto>;