using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Domain.Events;

namespace PetCare.Domain.Aggregates
{
    public sealed class Donation : AggregateRoot
    {
        // Value Objects
        public Money Amount { get; private set; }

        // Primitive properties
        public Guid? UserId { get; private set; }
        public Guid? ShelterId { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public DonationStatus Status { get; private set; }
        public string? TransactionId { get; private set; }
        public string? Purpose { get; private set; }
        public bool Recurring { get; private set; }
        public bool Anonymous { get; private set; }
        public DateTime DonationDate { get; private set; }
        public string? Report { get; private set; }

        // Parameterless constructor for EF Core
        private Donation()
        {
            Amount = Money.Zero();
        }

        private Donation(
            Guid? userId,
            Money amount,
            Guid? shelterId,
            Guid paymentMethodId,
            DonationStatus status,
            string? transactionId,
            string? purpose,
            bool recurring,
            bool anonymous,
            DateTime? donationDate,
            string? report) : base()
        {
            if (paymentMethodId == Guid.Empty)
                throw new ArgumentException("PaymentMethodId не може бути порожнім.", nameof(paymentMethodId));

            UserId = userId;
            Amount = amount;
            ShelterId = shelterId;
            PaymentMethodId = paymentMethodId;
            Status = status;
            TransactionId = transactionId;
            Purpose = purpose;
            Recurring = recurring;
            Anonymous = anonymous;
            DonationDate = donationDate ?? DateTime.UtcNow;
            Report = report;

            // Raise domain event
            AddDomainEvent(new DonationCreatedEvent(Id, Version, userId, amount.Amount, amount.Currency, shelterId, anonymous, recurring, purpose));
            MarkAsModified();
        }

        public static Donation Create(
            Guid? userId,
            decimal amount,
            string currency,
            Guid? shelterId,
            Guid paymentMethodId,
            DonationStatus status = DonationStatus.Pending,
            string? transactionId = null,
            string? purpose = null,
            bool recurring = false,
            bool anonymous = false,
            DateTime? donationDate = null,
            string? report = null)
        {
            return new Donation(
               userId,
               Money.Create(amount, currency),
               shelterId,
               paymentMethodId,
               status,
               transactionId,
               purpose,
               recurring,
               anonymous,
               donationDate,
               report
            );
        }

        public void UpdateReport(string? report)
        {
            Report = report;
            AddDomainEvent(new DonationReportUpdatedEvent(Id, Version, report, DateTime.UtcNow));
            MarkAsModified();
        }

        public void MarkAsCompleted(string? transactionId = null)
        {
            if (Status == DonationStatus.Completed) return;

            var previousStatus = Status;
            Status = DonationStatus.Completed;

            if (!string.IsNullOrWhiteSpace(transactionId))
                TransactionId = transactionId;

            AddDomainEvent(new DonationStatusChangedEvent(Id, Version, previousStatus, Status, transactionId));
            AddDomainEvent(new DonationCompletedEvent(Id, Version, UserId, Amount.Amount, Amount.Currency, ShelterId, transactionId, DateTime.UtcNow));

            // Schedule next donation if recurring
            if (Recurring)
            {
                var nextDonationDate = DonationDate.AddMonths(1); // Assuming monthly recurring
                AddDomainEvent(new RecurringDonationScheduledEvent(Id, Version, UserId, Amount.Amount, Amount.Currency, nextDonationDate));
            }

            MarkAsModified();
        }

        public void MarkAsFailed(string? reason = null)
        {
            if (Status == DonationStatus.Failed) return;

            var previousStatus = Status;
            Status = DonationStatus.Failed;

            AddDomainEvent(new DonationStatusChangedEvent(Id, Version, previousStatus, Status, null, reason));
            AddDomainEvent(new DonationFailedEvent(Id, Version, UserId, Amount.Amount, Amount.Currency, reason, DateTime.UtcNow));
            MarkAsModified();
        }

        public void ChangeStatus(DonationStatus newStatus, string? reason = null)
        {
            if (Status == newStatus) return;

            var previousStatus = Status;
            Status = newStatus;

            AddDomainEvent(new DonationStatusChangedEvent(Id, Version, previousStatus, newStatus, TransactionId, reason));

            // Handle specific status changes
            switch (newStatus)
            {
                case DonationStatus.Completed:
                    AddDomainEvent(new DonationCompletedEvent(Id, Version, UserId, Amount.Amount, Amount.Currency, ShelterId, TransactionId, DateTime.UtcNow));
                    break;
                case DonationStatus.Failed:
                    AddDomainEvent(new DonationFailedEvent(Id, Version, UserId, Amount.Amount, Amount.Currency, reason, DateTime.UtcNow));
                    break;
            }

            MarkAsModified();
        }

        public void SetTransactionId(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Transaction ID не може бути порожнім.", nameof(transactionId));

            TransactionId = transactionId;
            MarkAsModified();
        }

        // Business logic methods
        public bool IsCompleted => Status == DonationStatus.Completed;
        public bool IsFailed => Status == DonationStatus.Failed;
        public bool IsPending => Status == DonationStatus.Pending;
        public bool IsAnonymousDonation => Anonymous;
        public bool IsRecurringDonation => Recurring;
        public bool HasReport => !string.IsNullOrWhiteSpace(Report);

        public bool CanBeCompleted() => Status == DonationStatus.Pending;
        public bool CanBeFailed() => Status == DonationStatus.Pending;

        public Money GetAmount() => Amount;
    }
}
    }
}
