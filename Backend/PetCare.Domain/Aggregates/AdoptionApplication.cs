using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;

namespace PetCare.Domain.Aggregates
{
    public sealed class AdoptionApplication : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public Guid AnimalId { get; private set; }
        public AdoptionStatus Status { get; private set; }
        public DateTime ApplicationDate { get; private set; }
        public string? Comment { get; private set; }
        public string? AdminNotes { get; private set; }
        public Guid? ApprovedBy { get; private set; }
        public string? RejectionReason { get; private set; }

        private AdoptionApplication() { }

        private AdoptionApplication(
            Guid userId,
            Guid animalId,
            string? comment) : base()
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
            if (animalId == Guid.Empty)
                throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));

            UserId = userId;
            AnimalId = animalId;
            Comment = comment;
            Status = AdoptionStatus.Pending;
            ApplicationDate = DateTime.UtcNow;

            AddDomainEvent(new AdoptionApplicationSubmittedEvent(Id, Version, userId, animalId, comment));
            MarkAsModified();
        }

        public static AdoptionApplication Create(Guid userId, Guid animalId, string? comment)
            => new(userId, animalId, comment);

        public void Approve(Guid adminId)
        {
            if (Status != AdoptionStatus.Pending)
                throw new InvalidOperationException("Затверджуються лише ті заявки, які знаходяться на розгляді.");

            var previousStatus = Status;
            Status = AdoptionStatus.Approved;
            ApprovedBy = adminId;

            AddDomainEvent(new AdoptionApplicationStatusChangedEvent(Id, Version, previousStatus, Status, adminId));
            AddDomainEvent(new AdoptionApplicationApprovedEvent(Id, Version, UserId, AnimalId, adminId, DateTime.UtcNow));
            MarkAsModified();
        }

        public void Reject(Guid adminId, string reason)
        {
            if (Status != AdoptionStatus.Pending)
                throw new InvalidOperationException("Відхилити можна лише ті заявки, що перебувають на розгляді.");

            var previousStatus = Status;
            Status = AdoptionStatus.Rejected;
            RejectionReason = reason;

            AddDomainEvent(new AdoptionApplicationStatusChangedEvent(Id, Version, previousStatus, Status, adminId, reason));
            AddDomainEvent(new AdoptionApplicationRejectedEvent(Id, Version, UserId, AnimalId, adminId, reason, DateTime.UtcNow));
            MarkAsModified();
        }

        public void AddAdminNotes(string notes)
        {
            AdminNotes = notes;
            MarkAsModified();
        }

        // Business logic methods
        public bool IsPending => Status == AdoptionStatus.Pending;
        public bool IsApproved => Status == AdoptionStatus.Approved;
        public bool IsRejected => Status == AdoptionStatus.Rejected;
        public bool CanBeApproved => Status == AdoptionStatus.Pending;
        public bool CanBeRejected => Status == AdoptionStatus.Pending;
    }
}
