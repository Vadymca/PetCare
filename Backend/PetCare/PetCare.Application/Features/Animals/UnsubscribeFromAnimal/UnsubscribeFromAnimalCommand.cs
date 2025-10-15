namespace PetCare.Application.Features.Animals.UnsubscribeFromAnimal;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to unsubscribe a user from notifications or updates related to a specific animal.
/// </summary>
/// <param name="AnimalId">The unique identifier of the animal from which the user will be unsubscribed.</param>
/// <param name="UserId">The unique identifier of the user to be unsubscribed.</param>
public sealed record UnsubscribeFromAnimalCommand(
    Guid AnimalId,
    Guid UserId)
    : IRequest<UnsubscribeResultDto>;
