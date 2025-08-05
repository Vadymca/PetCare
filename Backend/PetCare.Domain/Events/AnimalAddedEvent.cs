// <copyright file="AnimalAddedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when a new animal is added to a shelter.
/// </summary>
public sealed record AnimalAddedEvent(Guid animalId, Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
