namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Repository interface for accessing user entities.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Retrieves all users with a specific role.
    /// </summary>
    /// <param name="role">The role to filter by.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of users.</returns>
    Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all users who have a subscription to a specific shelter.
    /// </summary>
    /// <param name="shelterId">The shelter's ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of users.</returns>
    Task<IReadOnlyList<User>> GetUsersByShelterSubscriptionAsync(Guid shelterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of users with optional filters.
    /// </summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="search">Optional search filter (name/email).</param>
    /// <param name="role">Optional role filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tuple with users and total count.</returns>
    Task<(IReadOnlyList<User> Users, int TotalCount)> GetUsersAsync(
        int page,
        int pageSize,
        string? search,
        string? role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the role of a user directly in the database without tracking conflicts.
    /// </summary>
    /// <param name="userId">The ID of the user whose role is being updated.</param>
    /// <param name="newRole">The new role to assign.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetUserRoleAsync(Guid userId, UserRole newRole, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all shelter subscriptions for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose shelter subscriptions are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="ShelterSubscription"/> associated with the user.</returns>
    Task<IReadOnlyList<ShelterSubscription>> GetUserShelterSubscriptionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all animal subscriptions for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose animal subscriptions are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="AnimalSubscription"/> associated with the user.</returns>
    Task<IReadOnlyList<AnimalSubscription>> GetUserAnimalSubscriptionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications submitted by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose adoption applications are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetUserAdoptionApplicationsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all events associated with a specific user (as participant or organizer).
    /// </summary>
    /// <param name="userId">The ID of the user whose events are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<IReadOnlyList<Event>> GetUserEventsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}