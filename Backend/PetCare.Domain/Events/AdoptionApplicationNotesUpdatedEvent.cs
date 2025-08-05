// <copyright file="AdoptionApplicationNotesUpdatedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when administrative notes are added or updated for an adoption application.
/// </summary>
public sealed record AdoptionApplicationNotesUpdatedEvent(Guid applicationId, Guid userId, string notes)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);