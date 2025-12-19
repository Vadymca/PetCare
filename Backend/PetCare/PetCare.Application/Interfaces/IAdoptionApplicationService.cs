namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Service for managing adoption applications.
/// </summary>
public interface IAdoptionApplicationService
{
    /// <summary>
    /// Creates a new adoption application.
    /// </summary>
    /// <param name="userId">The unique identifier of the user submitting the application.</param>
    /// <param name="animalId">The unique identifier of the animal to be adopted.</param>
    /// <param name="comment">An optional comment provided by the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created <see cref="AdoptionApplication"/>.</returns>
    Task<AdoptionApplication> CreateAsync(
        Guid userId,
        Guid animalId,
        string? comment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing adoption application.
    /// </summary>
    /// <param name="id">The unique identifier of the application to update.</param>
    /// <param name="comment">An optional comment to update.</param>
    /// <param name="adminNotes">Optional administrative notes to update.</param>
    /// <param name="curatorName">The name of the curator assigned to the application, if any.</param>
    /// <param name="curatorPhone">The phone number of the curator assigned to the application, if any.</param>
    /// <param name="meetingDate">The scheduled meeting date for the adoption, if any.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated <see cref="AdoptionApplication"/>.</returns>
    Task<AdoptionApplication> UpdateAsync(
        Guid id,
        string? comment,
        string? adminNotes,
        string? curatorName,
        string? curatorPhone,
        DateTime? meetingDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an adoption application by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the application to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an adoption application by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the application.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The <see cref="AdoptionApplication"/> if found; otherwise, <c>null</c>.</returns>
    Task<AdoptionApplication> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of all <see cref="AdoptionApplication"/> entities.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications submitted by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="AdoptionApplication"/> entities submitted by the user.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications filtered by status.
    /// </summary>
    /// <param name="status">The <see cref="AdoptionStatus"/> to filter by.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="AdoptionApplication"/> entities with the specified status.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetByStatusAsync(
        AdoptionStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a specific adoption application.
    /// </summary>
    /// <param name="id">The unique identifier of the application to approve.</param>
    /// <param name="adminId">The unique identifier of the administrator approving the application.</param>
    /// <param name="curatorName">The name of the curator assigned to the application, if any.</param>
    /// <param name="curatorPhone">The phone number of the curator assigned to the application, if any.</param>
    /// <param name="meetingDate">The scheduled meeting date for the adoption, if any.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ApproveAsync(
         Guid id,
         Guid adminId,
         string? curatorName = null,
         string? curatorPhone = null,
         DateTime? meetingDate = null,
         CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects a specific adoption application with a provided reason.
    /// </summary>
    /// <param name="id">The unique identifier of the application to reject.</param>
    /// <param name="reason">The reason for rejecting the application.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes the adoption process for a given application.
    /// If <paramref name="isAdopted"/> is true, the animal is marked as Adopted; otherwise Available.
    /// </summary>
    /// <param name="id">The ID of the adoption application.</param>
    /// <param name="isAdopted">Indicates if the adoption was successful.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CompleteAdoptionAsync(Guid id, bool isAdopted, CancellationToken cancellationToken = default);
}
