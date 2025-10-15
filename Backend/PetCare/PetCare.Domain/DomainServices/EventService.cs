namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing events created by users.
/// </summary>
public sealed class EventService : IEventService
{
    /// <inheritdoc/>
    public async Task AddEventAsync(User user, Event eventItem, Guid requestingUserId)
    {
        user.AddEvent(eventItem, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveEventAsync(User user, Guid eventId, Guid requestingUserId)
    {
        user.RemoveEvent(eventId, requestingUserId);
        await Task.CompletedTask;
    }
}
