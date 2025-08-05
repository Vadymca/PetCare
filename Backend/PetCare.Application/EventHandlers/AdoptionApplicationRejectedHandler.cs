// <copyright file="AdoptionApplicationRejectedHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PetCare.Application.EventHandlers;
using PetCare.Application.Common;
using PetCare.Domain.Events;

/// <summary>
/// Handles the event raised when an adoption application is rejected.
/// </summary>
public sealed class AdoptionApplicationRejectedHandler : IDomainEventHandler
{
    /// <inheritdoc/>
    public async Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var @event = (AdoptionApplicationRejectedEvent)domainEvent;

        // Логіка
        await Task.CompletedTask;
    }
}
