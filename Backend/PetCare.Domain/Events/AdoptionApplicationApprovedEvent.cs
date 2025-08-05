// <copyright file="AdoptionApplicationApprovedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when an adoption application is approved.
/// </summary>
public sealed record AdoptionApplicationApprovedEvent(Guid applicationId, Guid userId, Guid animalId, Guid approvedBy)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
