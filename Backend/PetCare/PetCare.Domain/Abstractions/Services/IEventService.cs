namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides asynchronous operations for managing events created by users.
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Adds a new event created by the user.
    /// </summary>
    /// <param name="user">The user who owns the event.</param>
    /// <param name="eventItem">The event to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddEventAsync(User user, Event eventItem, Guid requestingUserId);

    /// <summary>
    /// Removes an event created by the user.
    /// </summary>
    /// <param name="user">The user who owns the event.</param>
    /// <param name="eventId">The ID of the event to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveEventAsync(User user, Guid eventId, Guid requestingUserId);
}
