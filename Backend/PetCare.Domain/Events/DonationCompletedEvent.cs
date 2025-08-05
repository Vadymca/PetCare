// <copyright file="DonationCompletedEvent.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when a donation is successfully completed.
/// </summary>
public sealed record DonationCompletedEvent(Guid donationId, decimal amount)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
