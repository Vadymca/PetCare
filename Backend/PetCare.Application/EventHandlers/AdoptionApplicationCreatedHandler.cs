// <copyright file="AdoptionApplicationCreatedHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace PetCare.Application.EventHandlers;
using PetCare.Application.Common;
using PetCare.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles the <see cref="AdoptionApplicationCreatedEvent"/>.
/// </summary>
public sealed class AdoptionApplicationCreatedHandler : IDomainEventHandler
{
    /// <inheritdoc/>
    public async Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var @event = (AdoptionApplicationCreatedEvent)domainEvent;

        // Логіка
        await Task.CompletedTask;
    }
}
