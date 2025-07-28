using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Domain.Events;

namespace PetCare.Domain.Aggregates
{
    public sealed class User : AggregateRoot
    {
        // Value Objects
        public Email Email { get; private set; }
        public PersonName PersonName { get; private set; }
        public PhoneNumber Phone { get; private set; }

        // Primitive properties
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public Dictionary<string, string> Preferences { get; private set; }
        public int Points { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public string? ProfilePhoto { get; private set; }
        public string Language { get; private set; }

        // Parameterless constructor for EF Core
        private User()
        {
            Email = Email.Create("default@example.com");
            PersonName = PersonName.Create("Default", "User");
            Phone = PhoneNumber.Create("+380000000000");
            PasswordHash = string.Empty;
            Preferences = new Dictionary<string, string>();
            Language = "uk";
        }

        private User(
            Email email,
            string passwordHash,
            PersonName personName,
            PhoneNumber phone,
            UserRole role,
            Dictionary<string, string>? preferences,
            int points,
            DateTime? lastLogin,
            string? profilePhoto,
            string language) : base()
        {
            Email = email;
            PasswordHash = passwordHash;
            PersonName = personName;
            Phone = phone;
            Role = role;
            Preferences = preferences ?? new Dictionary<string, string>();
            Points = points;
            LastLogin = lastLogin;
            ProfilePhoto = profilePhoto;
            Language = language;

            // Raise domain event
            AddDomainEvent(new UserRegisteredEvent(Id, Version, email.Value, personName.FirstName, personName.LastName, role));
            MarkAsModified();
        }

        public static User Create(
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            string phone,
            UserRole role,
            Dictionary<string, string>? preferences = null,
            int points = 0,
            DateTime? lastLogin = null,
            string? profilePhoto = null,
            string language = "uk")
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Хеш-пароль не може бути порожнім", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(language) || language.Length > 10)
                throw new ArgumentException("Мова невірна", nameof(language));

            if (points < 0)
                throw new ArgumentException("Бали не можуть бути від'ємними", nameof(points));

            return new User(
                Email.Create(email),
                passwordHash,
                PersonName.Create(firstName, lastName),
                PhoneNumber.Create(phone),
                role,
                preferences,
                points,
                lastLogin,
                profilePhoto,
                language);
        }

        public void UpdateProfile(
            string? firstName = null,
            string? lastName = null,
            string? phone = null,
            string? profilePhoto = null,
            string? language = null)
        {
            var currentFirstName = firstName ?? PersonName.FirstName;
            var currentLastName = lastName ?? PersonName.LastName;

            if (!string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName))
                PersonName = PersonName.Create(currentFirstName, currentLastName);

            if (!string.IsNullOrWhiteSpace(phone)) Phone = PhoneNumber.Create(phone);
            if (!string.IsNullOrWhiteSpace(profilePhoto)) ProfilePhoto = profilePhoto;
            if (!string.IsNullOrWhiteSpace(language)) Language = language;

            AddDomainEvent(new UserProfileUpdatedEvent(Id, Version, firstName, lastName, phone, profilePhoto));
            MarkAsModified();
        }

        public void AwardPoints(int amount, string? reason = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Кількість балів повинна бути більше нуля.", nameof(amount));

            Points += amount;
            AddDomainEvent(new UserPointsAwardedEvent(Id, Version, amount, Points, reason));
            MarkAsModified();
        }

        public void ChangeRole(UserRole newRole, Guid? changedBy = null)
        {
            if (Role == newRole) return;

            var previousRole = Role;
            Role = newRole;

            AddDomainEvent(new UserRoleChangedEvent(Id, Version, previousRole, newRole, changedBy));
            MarkAsModified();
        }

        public void RecordLogin(DateTime loginTime, string? ipAddress = null)
        {
            LastLogin = loginTime;
            AddDomainEvent(new UserLoggedInEvent(Id, Version, loginTime, ipAddress));
            MarkAsModified();
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Хеш-пароль не може бути порожнім", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
            AddDomainEvent(new UserPasswordChangedEvent(Id, Version, DateTime.UtcNow));
            MarkAsModified();
        }

        public void ChangeEmail(string newEmail)
        {
            var previousEmail = Email.Value;
            Email = Email.Create(newEmail);

            AddDomainEvent(new UserEmailChangedEvent(Id, Version, previousEmail, newEmail));
            MarkAsModified();
        }

        public void UpdatePreferences(Dictionary<string, string> newPreferences)
        {
            Preferences = newPreferences ?? new Dictionary<string, string>();
            MarkAsModified();
        }

        // Business logic methods
        public string FullName => PersonName.FullName;
        public bool IsAdmin => Role == UserRole.Admin;
        public bool IsModerator => Role == UserRole.Moderator;
        public bool HasPoints => Points > 0;
        public bool HasRecentLogin => LastLogin.HasValue && LastLogin.Value > DateTime.UtcNow.AddDays(-30);

        public bool CanPerformAdminActions() => IsAdmin;
        public bool CanModerateContent() => IsAdmin || IsModerator;
    }
}
    }
}
