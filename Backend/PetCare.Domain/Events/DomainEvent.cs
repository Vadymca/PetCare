// <copyright file="DomainEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Represents a base domain event.
/// </summary>
public abstract record DomainEvent(Guid id, DateTime occurredAt);
