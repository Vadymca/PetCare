namespace PetCare.Application.Dtos.AnimalDtos;

using System;

/// <summary>
/// Represents a data transfer object containing information about a user's subscription to an animal.
/// </summary>
/// <param name="Id">The unique identifier for the animal subscription.</param>
/// <param name="UserId">The unique identifier of the user who is subscribed.</param>
/// <param name="AnimalId">The unique identifier of the animal to which the user is subscribed.</param>
/// <param name="SubscribedAt">The date and time when the subscription was created, in UTC.</param>
public sealed record AnimalSubscriptionDto(
 Guid Id,
 Guid UserId,
 Guid AnimalId,
 DateTime SubscribedAt);
