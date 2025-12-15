namespace PetCare.Domain.Aggregates;

using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;

/// <summary>
/// Represents an adoption application for an animal in the system.
/// </summary>
public sealed class AdoptionApplication : AggregateRoot
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
    /// Gets the current status of the adoption application.
    /// </summary>
    public AdoptionStatus Status { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was submitted.
    /// </summary>
    public DateTime ApplicationDate { get; private set; }

    /// <summary>
    /// Gets the date when the application was rejected, if any.
    /// </summary>
    public DateTime? RejectionDate { get; private set; }

    /// <summary>
    /// Gets the date when the adoption was finalized, if any.
    /// </summary>
    public DateTime? AdoptionDate { get; private set; }

    /// <summary>
    /// Gets the date of the scheduled meeting with the adopter, if any.
    /// </summary>
    public DateTime? MeetingDate { get; private set; }

    /// <summary>
    /// Gets the optional comment provided by the user. Can be null.
    /// </summary>
    public string? Comment { get; private set; }

    /// <summary>
    /// Gets the optional notes added by an administrator. Can be null.
    /// </summary>
    public string? AdminNotes { get; private set; }

    /// <summary>
    /// Gets the reason for rejection of the application, if any. Can be null.
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Gets name of the curator responsible for this application.
    /// </summary>
    public string? CuratorName { get; private set; }

    /// <summary>
    /// Gets phone number of the curator responsible for this application.
    /// </summary>
    public string? CuratorPhone { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the application was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user submitting the application.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user associated with the application (navigation property for EF Core).
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal for adoption.
    /// </summary>
    public Guid AnimalId { get; private set; }

    /// <summary>
    /// Gets the animal associated with the application (navigation property for EF Core).
    /// </summary>
    public Animal? Animal { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the administrator who approved the application, if any. Can be null.
    /// </summary>
    public Guid? ApprovedBy { get; private set; }

    /// <summary>
    /// Gets the admin user who approved the application (navigation property for EF Core).
    /// </summary>
    public User? ApprovedByUser { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the application is in Pending status.
    /// </summary>
    public bool IsPending => this.Status == AdoptionStatus.Pending;

    /// <summary>
    /// Gets a value indicating whether the application is Approved.
    /// </summary>
    public bool IsApproved => this.Status == AdoptionStatus.Approved;

    /// <summary>
    /// Gets a value indicating whether the application is Rejected.
    /// </summary>
    public bool IsRejected => this.Status == AdoptionStatus.Rejected;

    /// <summary>
    /// Gets a value indicating whether the application can be approved.
    /// </summary>
    public bool CanBeApproved => this.Status == AdoptionStatus.Pending;

    /// <summary>
    /// Gets a value indicating whether the application can be rejected.
    /// </summary>
    public bool CanBeRejected => this.Status == AdoptionStatus.Pending;

    /// <summary>
    /// Creates a new <see cref="AdoptionApplication"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user submitting the application.</param>
    /// <param name="animalId">The unique identifier of the animal for adoption.</param>
    /// <param name="comment">An optional comment provided by the user. Can be null.</param>
    /// <returns>A new instance of <see cref="AdoptionApplication"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> or <paramref name="animalId"/> is empty.</exception>
    public static AdoptionApplication Create(Guid userId, Guid animalId, string? comment)
    {
        var application = new AdoptionApplication(userId, animalId, comment);
        application.AddDomainEvent(new AdoptionApplicationCreatedEvent(application.Id, userId, animalId));
        return application;
    }

    /// <summary>
    /// Approves the adoption application and sets the approving administrator.
    /// </summary>
    /// <param name="adminId">The unique identifier of the administrator approving the application.</param>
    /// <param name="curatorName">Optional name of the curator to assign upon approval.</param>
    /// <param name="curatorPhone">Optional phone number of the curator to assign upon approval.</param>
    /// <exception cref="InvalidOperationException">Thrown when the application is not in the <see cref="AdoptionStatus.Pending"/> state.</exception>
    public void Approve(Guid adminId, string? curatorName = null, string? curatorPhone = null)
    {
        if (!this.CanBeApproved)
        {
            throw new InvalidOperationException("Затверджуються лише ті заявки, які знаходяться на розгляді.");
        }

        this.Status = AdoptionStatus.Approved;
        this.ApprovedBy = adminId;
        this.UpdatedAt = DateTime.UtcNow;

        // Призначаємо куратора через існуючий метод
        if (!string.IsNullOrWhiteSpace(curatorName) && !string.IsNullOrWhiteSpace(curatorPhone))
        {
            this.AssignCurator(curatorName, curatorPhone);
        }

        this.AddDomainEvent(new AdoptionApplicationApprovedEvent(this.Id, this.UserId, this.AnimalId, adminId));
    }

    /// <summary>
    /// Rejects the adoption application with the specified reason.
    /// </summary>
    /// <param name="reason">The reason for rejecting the application.</param>
    /// <exception cref="InvalidOperationException">Thrown when the application is not in the <see cref="AdoptionStatus.Pending"/> state.</exception>
    public void Reject(string reason)
    {
        if (!this.CanBeRejected)
        {
            throw new InvalidOperationException("Відхилити можна лише ті заявки, що перебувають на розгляді.");
        }

        this.Status = AdoptionStatus.Rejected;
        this.RejectionReason = reason;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new AdoptionApplicationRejectedEvent(this.Id, this.UserId, this.AnimalId, reason));
    }

    /// <summary>
    /// Schedules a meeting with the adopter.
    /// </summary>
    /// <param name="meetingDate">Meeting date and time.</param>
    public void ScheduleMeeting(DateTime meetingDate)
    {
        if (!this.IsPending)
        {
            throw new InvalidOperationException("Можна призначити зустріч лише для заявок на розгляді.");
        }

        this.MeetingDate = meetingDate;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Completes the adoption process.
    /// </summary>
    /// <param name="adoptionDate">Date of adoption.</param>
    public void CompleteAdoption(DateTime adoptionDate)
    {
        if (!this.IsApproved)
        {
            throw new InvalidOperationException("Лише схвалені заявки можуть бути завершені усиновленням.");
        }

        this.AdoptionDate = adoptionDate;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds or updates administrative notes for the application.
    /// </summary>
    /// <param name="notes">The administrative notes to add or update.</param>
    public void AddAdminNotes(string notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            throw new ArgumentException("Адміністративні нотатки не можуть бути порожніми.", nameof(notes));
        }

        this.AdminNotes = notes;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new AdoptionApplicationNotesUpdatedEvent(this.Id, this.UserId, notes));
    }

    /// <summary>
    /// Assigns a curator to the application.
    /// </summary>
    /// <param name="name">Curator name.</param>
    /// <param name="phone">Curator phone number.</param>
    public void AssignCurator(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ім'я куратора не може бути порожнім.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Телефон куратора не може бути порожнім.", nameof(phone));
        }

        this.CuratorName = name;
        this.CuratorPhone = phone;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the user comment for the adoption application.
    /// </summary>
    /// <param name="comment">The new comment to set.</param>
    public void UpdateComment(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new ArgumentException("Коментар не може бути порожнім.", nameof(comment));
        }

        this.Comment = comment;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new AdoptionApplicationNotesUpdatedEvent(this.Id, this.UserId, comment));
    }
}
