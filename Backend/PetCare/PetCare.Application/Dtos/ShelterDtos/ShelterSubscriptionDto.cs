namespace PetCare.Application.Dtos.ShelterDtos;

using System;

/// <summary>
/// Represents a user's subscription to a specific shelter.
/// </summary>
/// <param name="Id">The unique identifier of the subscription.</param>
/// <param name="UserId">The ID of the subscribed user.</param>
/// <param name="ShelterId">The ID of the shelter.</param>
/// <param name="SubscribedAt">The UTC timestamp when the subscription occurred.</param>
public sealed record ShelterSubscriptionDto(
    Guid Id,
    Guid UserId,
    Guid ShelterId,
    DateTime SubscribedAt);
