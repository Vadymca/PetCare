// <copyright file="AdoptionApplication.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents an adoption application for an animal in the system.
/// </summary>
public sealed class AdoptionApplication : BaseEntity
{
    private AdoptionApplication()
    {
    }

    private AdoptionApplication(
        Guid userId,
        Guid animalId,
        string? comment)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));
        }

        this.UserId = userId;
        this.AnimalId = animalId;
        this.Comment = comment;
        this.Status = AdoptionStatus.Pending;
        this.ApplicationDate = DateTime.UtcNow;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the user submitting the application.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal for adoption.
    /// </summary>
    public Guid AnimalId { get; private set; }

    /// <summary>
    /// Gets the current status of the adoption application.
    /// </summary>
    public AdoptionStatus Status { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was submitted.
    /// </summary>
    public DateTime ApplicationDate { get; private set; }

    /// <summary>
    /// Gets the optional comment provided by the user. Can be null.
    /// </summary>
    public string? Comment { get; private set; }

    /// <summary>
    /// Gets the optional notes added by an administrator. Can be null.
    /// </summary>
    public string? AdminNotes { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the administrator who approved the application, if any. Can be null.
    /// </summary>
    public Guid? ApprovedBy { get; private set; }

    /// <summary>
    /// Gets the reason for rejection of the application, if any. Can be null.
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="AdoptionApplication"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user submitting the application.</param>
    /// <param name="animalId">The unique identifier of the animal for adoption.</param>
    /// <param name="comment">An optional comment provided by the user. Can be null.</param>
    /// <returns>A new instance of <see cref="AdoptionApplication"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> or <paramref name="animalId"/> is empty.</exception>
    public static AdoptionApplication Create(Guid userId, Guid animalId, string? comment)
        => new AdoptionApplication(userId, animalId, comment);

    /// <summary>
    /// Approves the adoption application and sets the approving administrator.
    /// </summary>
    /// <param name="adminId">The unique identifier of the administrator approving the application.</param>
    /// <exception cref="InvalidOperationException">Thrown when the application is not in the <see cref="AdoptionStatus.Pending"/> state.</exception>
    public void Approve(Guid adminId)
    {
        if (this.Status != AdoptionStatus.Pending)
        {
            throw new InvalidOperationException("Затверджуються лише ті заявки, які знаходяться на розгляді.");
        }

        this.Status = AdoptionStatus.Approved;
        this.ApprovedBy = adminId;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Rejects the adoption application with the specified reason.
    /// </summary>
    /// <param name="reason">The reason for rejecting the application.</param>
    /// <exception cref="InvalidOperationException">Thrown when the application is not in the <see cref="AdoptionStatus.Pending"/> state.</exception>
    public void Reject(string reason)
    {
        if (this.Status != AdoptionStatus.Pending)
        {
            throw new InvalidOperationException("Відхилити можна лише ті заявки, що перебувають на розгляді.");
        }

        this.Status = AdoptionStatus.Rejected;
        this.RejectionReason = reason;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds or updates administrative notes for the application.
    /// </summary>
    /// <param name="notes">The administrative notes to add or update.</param>
    public void AddAdminNotes(string notes)
    {
        this.AdminNotes = notes;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
