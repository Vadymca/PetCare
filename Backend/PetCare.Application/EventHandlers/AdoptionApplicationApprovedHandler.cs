// <copyright file="AdoptionApplicationApprovedHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PetCare.Application.EventHandlers;
using PetCare.Application.Common;
using PetCare.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles the <see cref="AdoptionApplicationApprovedEvent"/>.
/// </summary>
public sealed class AdoptionApplicationApprovedHandler : IDomainEventHandler
{
    /// <inheritdoc/>
    public async Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var @event = (AdoptionApplicationApprovedEvent)domainEvent;

        // Логіка
        await Task.CompletedTask;
    }
}
