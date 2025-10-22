namespace PetCare.Application.Features.Animals.GetAnimalSubscriptions;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a request to retrieve all animals the user is subscribed to.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose animal subscriptions are being retrieved.</param>
public sealed record GetAnimalSubscriptionsCommand(Guid UserId)
    : IRequest<IReadOnlyList<AnimalListDto>>;
