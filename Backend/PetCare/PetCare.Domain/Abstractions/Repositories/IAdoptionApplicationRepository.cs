namespace PetCare.Domain.Abstractions.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;

/// <summary>
/// Represents a repository interface for managing <see cref="AdoptionApplication"/> entities.
/// </summary>
public interface IAdoptionApplicationRepository : IRepository<AdoptionApplication>
{
    /// <summary>
    /// Retrieves all adoption applications submitted by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A read-only list of adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications that are currently pending review.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A read-only list of pending adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetPendingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications that have been approved.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A read-only list of approved adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetApprovedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications that have been rejected.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A read-only list of rejected adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetRejectedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a specific adoption application.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application to approve.</param>
    /// <param name="adminId">The unique identifier of the administrator approving the application.</param>
    /// <param name="curatorName">The name of the curator (optional).</param>
    /// <param name="curatorPhone">The phone number of the curator (optional).</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ApproveAsync(
        Guid applicationId,
        Guid adminId,
        string? curatorName = null,
        string? curatorPhone = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects a specific adoption application with a provided reason.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application to reject.</param>
    /// <param name="reason">The reason for rejection.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RejectAsync(Guid applicationId, string reason, CancellationToken cancellationToken = default);
}
