// <copyright file="AdoptionApplicationRejectedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when an adoption application is rejected.
/// </summary>
public sealed record AdoptionApplicationRejectedEvent(Guid applicationId, Guid userId, Guid animalId, string rejectionReason)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
