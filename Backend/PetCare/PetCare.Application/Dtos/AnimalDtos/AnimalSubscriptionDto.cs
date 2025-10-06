namespace PetCare.Application.Dtos.AnimalDtos;
using System;

public sealed record AnimalSubscriptionDto(
 Guid Id,
 Guid UserId,
 Guid AnimalId,
 DateTime SubscribedAt);
