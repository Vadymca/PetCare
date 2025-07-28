using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Domain.Events;

namespace PetCare.Domain.Aggregates
{
    public sealed class Animal : AggregateRoot
    {
        // Value Objects
        public Slug Slug { get; private set; }
        public Name Name { get; private set; }
        public PhysicalCharacteristics PhysicalCharacteristics { get; private set; }
        public MicrochipId? MicrochipId { get; private set; }

        // Primitive properties
        public DateTime? Birthday { get; private set; }
        public AnimalGender Gender { get; private set; }
        public string? Description { get; private set; }
        public string? HealthStatus { get; private set; }
        public List<string> Photos { get; private set; } = new();
        public List<string> Videos { get; private set; } = new();
        public AnimalStatus Status { get; private set; }
        public string? AdoptionRequirements { get; private set; }
        public int IdNumber { get; private set; }
        public bool IsSterilized { get; private set; }
        public bool HaveDocuments { get; private set; }

        // Foreign keys
        public Guid UserId { get; private set; }
        public Guid BreedId { get; private set; }
        public Guid ShelterId { get; private set; }

        // Navigation properties
        public User? User { get; private set; }
        public Breed? Breed { get; private set; }
        public Shelter? Shelter { get; private set; }

        // Parameterless constructor for EF Core
        private Animal()
        {
            Slug = Slug.Create("default");
            Name = Name.Create("Default");
            PhysicalCharacteristics = PhysicalCharacteristics.Create();
        }

        private Animal(
            Slug slug,
            Guid userId,
            Name name,
            Guid breedId,
            DateTime? birthday,
            AnimalGender gender,
            string? description,
            string? healthStatus,
            List<string>? photos,
            List<string>? videos,
            Guid shelterId,
            AnimalStatus status,
            string? adoptionRequirements,
            MicrochipId? microchipId,
            int idNumber,
            PhysicalCharacteristics physicalCharacteristics,
            bool isSterilized,
            bool haveDocuments) : base()
        {
            Slug = slug;
            UserId = userId;
            Name = name;
            BreedId = breedId;
            Birthday = birthday;
            Gender = gender;
            Description = description;
            HealthStatus = healthStatus;
            Photos = photos ?? new List<string>();
            Videos = videos ?? new List<string>();
            ShelterId = shelterId;
            Status = status;
            AdoptionRequirements = adoptionRequirements;
            MicrochipId = microchipId;
            IdNumber = idNumber;
            PhysicalCharacteristics = physicalCharacteristics;
            IsSterilized = isSterilized;
            HaveDocuments = haveDocuments;

            // Raise domain event
            AddDomainEvent(new AnimalCreatedEvent(Id, Version, name.Value, breedId, shelterId, status));
            MarkAsModified();
        }

        public static Animal Create(
            string slug,
            Guid userId,
            string name,
            Guid breedId,
            DateTime? birthday,
            AnimalGender gender,
            string? description,
            string? healthStatus,
            List<string>? photos,
            List<string>? videos,
            Guid shelterId,
            AnimalStatus status,
            string? adoptionRequirements,
            string? microchipId,
            int idNumber,
            float? weight,
            float? height,
            string? color,
            bool isSterilized,
            bool haveDocuments)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId не може бути порожнім.", nameof(userId));
            if (breedId == Guid.Empty)
                throw new ArgumentException("BreedId не може бути порожнім.", nameof(breedId));
            if (shelterId == Guid.Empty)
                throw new ArgumentException("ShelterId не може бути порожнім.", nameof(shelterId));
            if (idNumber <= 0)
                throw new ArgumentException("IdNumber повинен бути більше нуля.", nameof(idNumber));

            return new Animal(
                Slug.Create(slug),
                userId,
                Name.Create(name),
                breedId,
                birthday,
                gender,
                description,
                healthStatus,
                photos,
                videos,
                shelterId,
                status,
                adoptionRequirements,
                microchipId is not null ? MicrochipId.Create(microchipId) : null,
                idNumber,
                PhysicalCharacteristics.Create(weight, height, color),
                isSterilized,
                haveDocuments
            );
        }

        public void UpdateBasicInfo(
            string? name = null,
            DateTime? birthday = null,
            AnimalGender? gender = null,
            string? description = null,
            List<string>? photos = null,
            List<string>? videos = null,
            string? adoptionRequirements = null,
            bool? isSterilized = null,
            bool? haveDocuments = null)
        {
            if (name is not null) Name = Name.Create(name);
            if (birthday is not null) Birthday = birthday;
            if (gender is not null) Gender = gender.Value;
            if (description is not null) Description = description;
            if (photos is not null) Photos = photos;
            if (videos is not null) Videos = videos;
            if (adoptionRequirements is not null) AdoptionRequirements = adoptionRequirements;
            if (isSterilized is not null) IsSterilized = isSterilized.Value;
            if (haveDocuments is not null) HaveDocuments = haveDocuments.Value;

            MarkAsModified();
        }

        public void UpdateHealthStatus(string? newHealthStatus)
        {
            var previousHealthStatus = HealthStatus;
            HealthStatus = newHealthStatus;

            AddDomainEvent(new AnimalHealthStatusUpdatedEvent(Id, Version, previousHealthStatus, newHealthStatus));
            MarkAsModified();
        }

        public void ChangeStatus(AnimalStatus newStatus, string? reason = null)
        {
            if (Status == newStatus) return;

            var previousStatus = Status;
            Status = newStatus;

            AddDomainEvent(new AnimalStatusChangedEvent(Id, Version, previousStatus, newStatus, reason));

            // Special case for adoption
            if (newStatus == AnimalStatus.Adopted)
            {
                AddDomainEvent(new AnimalAdoptedEvent(Id, Version, UserId, DateTime.UtcNow));
            }

            MarkAsModified();
        }

        public void UpdatePhysicalCharacteristics(float? weight = null, float? height = null, string? color = null)
        {
            PhysicalCharacteristics = PhysicalCharacteristics.Create(
                weight ?? PhysicalCharacteristics.Weight,
                height ?? PhysicalCharacteristics.Height,
                color ?? PhysicalCharacteristics.Color);

            AddDomainEvent(new AnimalPhysicalCharacteristicsUpdatedEvent(Id, Version, weight, height, color));
            MarkAsModified();
        }

        public void AddMicrochip(string microchipId)
        {
            if (MicrochipId is not null)
                throw new InvalidOperationException("Тварина вже має мікрочіп.");

            MicrochipId = MicrochipId.Create(microchipId);
            AddDomainEvent(new AnimalMicrochipAddedEvent(Id, Version, microchipId));
            MarkAsModified();
        }

        public void RemoveMicrochip()
        {
            MicrochipId = null;
            MarkAsModified();
        }

        // Business logic methods
        public bool IsAvailableForAdoption => Status == AnimalStatus.Available;
        public bool IsAdopted => Status == AnimalStatus.Adopted;
        public bool HasMicrochip => MicrochipId is not null;

        public int AgeInMonths
        {
            get
            {
                if (!Birthday.HasValue) return 0;
                var today = DateTime.Today;
                var age = today.Year - Birthday.Value.Year;
                if (Birthday.Value.Date > today.AddYears(-age)) age--;
                return age * 12 + (today.Month - Birthday.Value.Month);
            }
        }
    }
}
    }
}
