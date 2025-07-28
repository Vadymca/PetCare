using PetCare.Domain.Common;
using PetCare.Domain.Enums;

namespace PetCare.Domain.Events
{
    public sealed class AdoptionApplicationSubmittedEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public Guid AnimalId { get; }
        public string? Comment { get; }

        public AdoptionApplicationSubmittedEvent(Guid aggregateId, int aggregateVersion, Guid userId, Guid animalId, string? comment)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            AnimalId = animalId;
            Comment = comment;
        }
    }

    public sealed class AdoptionApplicationStatusChangedEvent : DomainEventBase
    {
        public AdoptionStatus PreviousStatus { get; }
        public AdoptionStatus NewStatus { get; }
        public Guid? ReviewedBy { get; }
        public string? Reason { get; }

        public AdoptionApplicationStatusChangedEvent(Guid aggregateId, int aggregateVersion, AdoptionStatus previousStatus, AdoptionStatus newStatus, Guid? reviewedBy = null, string? reason = null)
            : base(aggregateId, aggregateVersion)
        {
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            ReviewedBy = reviewedBy;
            Reason = reason;
        }
    }

    public sealed class AdoptionApplicationApprovedEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public Guid AnimalId { get; }
        public Guid ApprovedBy { get; }
        public DateTime ApprovedAt { get; }

        public AdoptionApplicationApprovedEvent(Guid aggregateId, int aggregateVersion, Guid userId, Guid animalId, Guid approvedBy, DateTime approvedAt)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            AnimalId = animalId;
            ApprovedBy = approvedBy;
            ApprovedAt = approvedAt;
        }
    }

    public sealed class AdoptionApplicationRejectedEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public Guid AnimalId { get; }
        public Guid RejectedBy { get; }
        public string? RejectionReason { get; }
        public DateTime RejectedAt { get; }

        public AdoptionApplicationRejectedEvent(Guid aggregateId, int aggregateVersion, Guid userId, Guid animalId, Guid rejectedBy, string? rejectionReason, DateTime rejectedAt)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            AnimalId = animalId;
            RejectedBy = rejectedBy;
            RejectionReason = rejectionReason;
            RejectedAt = rejectedAt;
        }
    }
}
