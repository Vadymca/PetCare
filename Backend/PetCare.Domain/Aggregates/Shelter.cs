using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;
using PetCare.Domain.Events;

namespace PetCare.Domain.Aggregates
{
    public sealed class Shelter : AggregateRoot
    {
        // Value Objects
        public Slug Slug { get; private set; }
        public Name Name { get; private set; }
        public Address Address { get; private set; }
        public Coordinates Coordinates { get; private set; }
        public ContactInfo ContactInfo { get; private set; }

        // Primitive properties
        public string? Description { get; private set; }
        public int Capacity { get; private set; }
        public int CurrentOccupancy { get; private set; }
        public List<string> Photos { get; private set; } = new();
        public string? VirtualTourUrl { get; private set; }
        public string? WorkingHours { get; private set; }
        public Dictionary<string, string> SocialMedia { get; private set; } = new();
        public Guid? ManagerId { get; private set; }

        // Navigation properties
        public User? Manager { get; private set; }

        // Parameterless constructor for EF Core
        private Shelter()
        {
            Slug = Slug.Create("default");
            Name = Name.Create("Default");
            Address = Address.Create("вул. Головна, 1, м. Київ");
            Coordinates = Coordinates.Create(50.4501, 30.5234);
            ContactInfo = ContactInfo.Create("default@example.com", "+380000000000");
        }

        private Shelter(
            Slug slug,
            Name name,
            Address address,
            Coordinates coordinates,
            ContactInfo contactInfo,
            string? description,
            int capacity,
            int currentOccupancy,
            List<string>? photos,
            string? virtualTourUrl,
            string? workingHours,
            Dictionary<string, string>? socialMedia,
            Guid? managerId) : base()
        {
            Slug = slug;
            Name = name;
            Address = address;
            Coordinates = coordinates;
            ContactInfo = contactInfo;
            Description = description;
            Capacity = capacity;
            CurrentOccupancy = currentOccupancy;
            Photos = photos ?? new List<string>();
            VirtualTourUrl = virtualTourUrl;
            WorkingHours = workingHours;
            SocialMedia = socialMedia ?? new Dictionary<string, string>();
            ManagerId = managerId;

            // Raise domain event
            AddDomainEvent(new ShelterCreatedEvent(Id, Version, name.Value, address.Value, managerId, capacity));
            MarkAsModified();
        }

        public static Shelter Create(
            string slug,
            string name,
            string address,
            double latitude,
            double longitude,
            string contactPhone,
            string contactEmail,
            string? description,
            int capacity,
            int currentOccupancy = 0,
            List<string>? photos = null,
            string? virtualTourUrl = null,
            string? workingHours = null,
            Dictionary<string, string>? socialMedia = null,
            Guid? managerId = null)
        {
            if (capacity <= 0)
                throw new ArgumentException("Ємність притулку повинна бути більше нуля.", nameof(capacity));
            if (currentOccupancy < 0)
                throw new ArgumentException("Поточна заповненість не може бути від'ємною.", nameof(currentOccupancy));
            if (currentOccupancy > capacity)
                throw new ArgumentException("Поточна заповненість не може перевищувати ємність.", nameof(currentOccupancy));

            return new Shelter(
                Slug.Create(slug),
                Name.Create(name),
                Address.Create(address),
                Coordinates.Create(latitude, longitude),
                ContactInfo.Create(contactEmail, contactPhone),
                description,
                capacity,
                currentOccupancy,
                photos,
                virtualTourUrl,
                workingHours,
                socialMedia,
                managerId);
        }

        public void UpdateBasicInfo(
            string? name = null,
            string? address = null,
            double? latitude = null,
            double? longitude = null,
            string? description = null,
            List<string>? photos = null,
            string? virtualTourUrl = null,
            Dictionary<string, string>? socialMedia = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = Name.Create(name);
            if (!string.IsNullOrWhiteSpace(address)) Address = Address.Create(address);
            if (latitude.HasValue && longitude.HasValue)
                Coordinates = Coordinates.Create(latitude.Value, longitude.Value);
            if (description != null) Description = description;
            if (photos != null) Photos = photos;
            if (virtualTourUrl != null) VirtualTourUrl = virtualTourUrl;
            if (socialMedia != null) SocialMedia = socialMedia;

            MarkAsModified();
        }

        public void UpdateContactInfo(string? newEmail = null, string? newPhone = null, string? alternativeContact = null)
        {
            var currentEmail = newEmail ?? ContactInfo.Email.Value;
            var currentPhone = newPhone ?? ContactInfo.PhoneNumber.Value;
            var currentAlt = alternativeContact ?? ContactInfo.AlternativeContact;

            ContactInfo = ContactInfo.Create(currentEmail, currentPhone, currentAlt);

            AddDomainEvent(new ShelterContactInfoUpdatedEvent(Id, Version, newPhone, newEmail));
            MarkAsModified();
        }

        public void UpdateWorkingHours(string? newWorkingHours)
        {
            WorkingHours = newWorkingHours;
            AddDomainEvent(new ShelterWorkingHoursUpdatedEvent(Id, Version, newWorkingHours));
            MarkAsModified();
        }

        public void ChangeCapacity(int newCapacity)
        {
            if (newCapacity <= 0)
                throw new ArgumentException("Ємність притулку повинна бути більше нуля.", nameof(newCapacity));
            if (newCapacity < CurrentOccupancy)
                throw new InvalidOperationException("Нова ємність не може бути меншою за поточну заповненість.");

            var previousCapacity = Capacity;
            Capacity = newCapacity;

            AddDomainEvent(new ShelterCapacityChangedEvent(Id, Version, previousCapacity, newCapacity));
            MarkAsModified();
        }

        public void UpdateOccupancy(int newOccupancy)
        {
            if (newOccupancy < 0)
                throw new ArgumentException("Заповненість не може бути від'ємною.", nameof(newOccupancy));
            if (newOccupancy > Capacity)
                throw new InvalidOperationException("Заповненість не може перевищувати ємність притулку.");

            var previousOccupancy = CurrentOccupancy;
            CurrentOccupancy = newOccupancy;

            AddDomainEvent(new ShelterOccupancyChangedEvent(Id, Version, previousOccupancy, newOccupancy, IsAtCapacity));
            MarkAsModified();
        }

        public void ChangeManager(Guid? newManagerId)
        {
            var previousManagerId = ManagerId;
            ManagerId = newManagerId;

            AddDomainEvent(new ShelterManagerChangedEvent(Id, Version, previousManagerId, newManagerId));
            MarkAsModified();
        }

        // Business logic methods
        public bool IsAtCapacity => CurrentOccupancy >= Capacity;
        public int AvailableSpaces => Capacity - CurrentOccupancy;
        public double OccupancyPercentage => Capacity > 0 ? (double)CurrentOccupancy / Capacity * 100 : 0;

        public bool CanAccommodateAnimals(int count) => CurrentOccupancy + count <= Capacity;

        public void AddAnimal()
        {
            if (IsAtCapacity)
                throw new InvalidOperationException("Притулок заповнений до максимуму.");

            UpdateOccupancy(CurrentOccupancy + 1);
        }

        public void RemoveAnimal()
        {
            if (CurrentOccupancy == 0)
                throw new InvalidOperationException("Немає тварин для видалення.");

            UpdateOccupancy(CurrentOccupancy - 1);
        }
    }
}
    }
}
