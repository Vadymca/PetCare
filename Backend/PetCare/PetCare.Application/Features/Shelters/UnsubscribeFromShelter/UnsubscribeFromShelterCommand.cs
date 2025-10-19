namespace PetCare.Application.Features.Shelters.UnsubscribeFromShelter;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to unsubscribe a user from notifications or updates related to a specific shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the user will be unsubscribed.</param>
/// <param name="UserId">The unique identifier of the user to be unsubscribed from the shelter.</param>
public sealed record UnsubscribeFromShelterCommand(
Guid ShelterId,
Guid UserId)
: IRequest<UnsubscribeResultDto>;
