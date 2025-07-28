using PetCare.Domain.Common;
using PetCare.Domain.Enums;

namespace PetCare.Domain.Events
{
    public sealed class DonationCreatedEvent : DomainEventBase
    {
        public Guid? UserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public Guid? ShelterId { get; }
        public bool IsAnonymous { get; }
        public bool IsRecurring { get; }
        public string? Purpose { get; }

        public DonationCreatedEvent(Guid aggregateId, int aggregateVersion, Guid? userId, decimal amount, string currency, Guid? shelterId, bool isAnonymous, bool isRecurring, string? purpose)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            Amount = amount;
            Currency = currency;
            ShelterId = shelterId;
            IsAnonymous = isAnonymous;
            IsRecurring = isRecurring;
            Purpose = purpose;
        }
    }

    public sealed class DonationStatusChangedEvent : DomainEventBase
    {
        public DonationStatus PreviousStatus { get; }
        public DonationStatus NewStatus { get; }
        public string? TransactionId { get; }
        public string? Reason { get; }

        public DonationStatusChangedEvent(Guid aggregateId, int aggregateVersion, DonationStatus previousStatus, DonationStatus newStatus, string? transactionId = null, string? reason = null)
            : base(aggregateId, aggregateVersion)
        {
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            TransactionId = transactionId;
            Reason = reason;
        }
    }

    public sealed class DonationCompletedEvent : DomainEventBase
    {
        public Guid? UserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public Guid? ShelterId { get; }
        public string? TransactionId { get; }
        public DateTime CompletedAt { get; }

        public DonationCompletedEvent(Guid aggregateId, int aggregateVersion, Guid? userId, decimal amount, string currency, Guid? shelterId, string? transactionId, DateTime completedAt)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            Amount = amount;
            Currency = currency;
            ShelterId = shelterId;
            TransactionId = transactionId;
            CompletedAt = completedAt;
        }
    }

    public sealed class DonationFailedEvent : DomainEventBase
    {
        public Guid? UserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string? Reason { get; }
        public DateTime FailedAt { get; }

        public DonationFailedEvent(Guid aggregateId, int aggregateVersion, Guid? userId, decimal amount, string currency, string? reason, DateTime failedAt)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            Amount = amount;
            Currency = currency;
            Reason = reason;
            FailedAt = failedAt;
        }
    }

    public sealed class DonationReportUpdatedEvent : DomainEventBase
    {
        public string? NewReport { get; }
        public DateTime UpdatedAt { get; }

        public DonationReportUpdatedEvent(Guid aggregateId, int aggregateVersion, string? newReport, DateTime updatedAt)
            : base(aggregateId, aggregateVersion)
        {
            NewReport = newReport;
            UpdatedAt = updatedAt;
        }
    }

    public sealed class RecurringDonationScheduledEvent : DomainEventBase
    {
        public Guid? UserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public DateTime NextDonationDate { get; }

        public RecurringDonationScheduledEvent(Guid aggregateId, int aggregateVersion, Guid? userId, decimal amount, string currency, DateTime nextDonationDate)
            : base(aggregateId, aggregateVersion)
        {
            UserId = userId;
            Amount = amount;
            Currency = currency;
            NextDonationDate = nextDonationDate;
        }
    }
}
