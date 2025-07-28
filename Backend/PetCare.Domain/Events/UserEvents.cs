using PetCare.Domain.Common;
using PetCare.Domain.Enums;

namespace PetCare.Domain.Events
{
    public sealed class UserRegisteredEvent : DomainEventBase
    {
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRole Role { get; }

        public UserRegisteredEvent(Guid aggregateId, int aggregateVersion, string email, string firstName, string lastName, UserRole role)
            : base(aggregateId, aggregateVersion)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }
    }

    public sealed class UserProfileUpdatedEvent : DomainEventBase
    {
        public string? NewFirstName { get; }
        public string? NewLastName { get; }
        public string? NewPhone { get; }
        public string? NewProfilePhoto { get; }

        public UserProfileUpdatedEvent(Guid aggregateId, int aggregateVersion, string? newFirstName, string? newLastName, string? newPhone, string? newProfilePhoto)
            : base(aggregateId, aggregateVersion)
        {
            NewFirstName = newFirstName;
            NewLastName = newLastName;
            NewPhone = newPhone;
            NewProfilePhoto = newProfilePhoto;
        }
    }

    public sealed class UserPointsAwardedEvent : DomainEventBase
    {
        public int PointsAwarded { get; }
        public int TotalPoints { get; }
        public string? Reason { get; }

        public UserPointsAwardedEvent(Guid aggregateId, int aggregateVersion, int pointsAwarded, int totalPoints, string? reason = null)
            : base(aggregateId, aggregateVersion)
        {
            PointsAwarded = pointsAwarded;
            TotalPoints = totalPoints;
            Reason = reason;
        }
    }

    public sealed class UserRoleChangedEvent : DomainEventBase
    {
        public UserRole PreviousRole { get; }
        public UserRole NewRole { get; }
        public Guid? ChangedBy { get; }

        public UserRoleChangedEvent(Guid aggregateId, int aggregateVersion, UserRole previousRole, UserRole newRole, Guid? changedBy = null)
            : base(aggregateId, aggregateVersion)
        {
            PreviousRole = previousRole;
            NewRole = newRole;
            ChangedBy = changedBy;
        }
    }

    public sealed class UserLoggedInEvent : DomainEventBase
    {
        public DateTime LoginTime { get; }
        public string? IpAddress { get; }

        public UserLoggedInEvent(Guid aggregateId, int aggregateVersion, DateTime loginTime, string? ipAddress = null)
            : base(aggregateId, aggregateVersion)
        {
            LoginTime = loginTime;
            IpAddress = ipAddress;
        }
    }

    public sealed class UserPasswordChangedEvent : DomainEventBase
    {
        public DateTime ChangedAt { get; }

        public UserPasswordChangedEvent(Guid aggregateId, int aggregateVersion, DateTime changedAt)
            : base(aggregateId, aggregateVersion)
        {
            ChangedAt = changedAt;
        }
    }

    public sealed class UserEmailChangedEvent : DomainEventBase
    {
        public string PreviousEmail { get; }
        public string NewEmail { get; }

        public UserEmailChangedEvent(Guid aggregateId, int aggregateVersion, string previousEmail, string newEmail)
            : base(aggregateId, aggregateVersion)
        {
            PreviousEmail = previousEmail;
            NewEmail = newEmail;
        }
    }
}
