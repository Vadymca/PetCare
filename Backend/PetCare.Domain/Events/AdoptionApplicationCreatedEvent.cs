// <copyright file="AdoptionApplicationCreatedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when a new adoption application is created.
/// </summary>
public sealed record AdoptionApplicationCreatedEvent(Guid applicationId, Guid userId, Guid animalId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
