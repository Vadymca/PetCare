namespace PetCare.Application.Features.Shelters.SubscribeToShelter;

using System;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to subscribe a user to updates for a specific shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter to subscribe to.</param>
/// <param name="UserId">The unique identifier of the user who will be subscribed.</param>
public sealed record SubscribeToShelterCommand(
    Guid ShelterId,
    Guid UserId)
    : IRequest<ShelterSubscriptionDto>;
