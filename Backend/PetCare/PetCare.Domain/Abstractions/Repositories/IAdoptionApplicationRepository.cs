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
    /// Returns all adoption applications sorted by creation date (newest first).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of adoption applications ordered by newest first.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetAllOrderedByNewestAsync(
        CancellationToken cancellationToken = default);

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
    /// <param name="meetingDate">The date of the meeting (optional).</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ApproveAsync(
        Guid applicationId,
        Guid adminId,
        string? curatorName = null,
        string? curatorPhone = null,
        DateTime? meetingDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects a specific adoption application with a provided reason.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application to reject.</param>
    /// <param name="reason">The reason for rejection.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RejectAsync(Guid applicationId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new <see cref="AdoptionApplication"/> and reserves the associated <see cref="Animal"/>.
    /// </summary>
    /// <param name="userId">The ID of the user creating the adoption application.</param>
    /// <param name="animalId">The ID of the animal to be adopted.</param>
    /// <param name="comment">An optional comment provided by the user.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created <see cref="AdoptionApplication"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the animal does not exist or is already reserved or adopted.
    /// </exception>
    Task<AdoptionApplication> CreateWithAnimalReservationAsync(
    Guid userId,
    Guid animalId,
    string? comment,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an adoption application and releases the associated animal if it was reserved.
    /// </summary>
    /// <param name="applicationId">The ID of the adoption application to delete.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the application or animal is not found.</exception>
    Task DeleteWithAnimalReleaseAsync(Guid applicationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the adoption as completed and updates the related animal status.
    /// </summary>
    /// <param name="applicationId">The ID of the adoption application.</param>
    /// <param name="isAdopted">If true, the animal is marked as Adopted; otherwise Available.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CompleteAdoptionAsync(Guid applicationId, bool isAdopted, CancellationToken cancellationToken = default);
}
