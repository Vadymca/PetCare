using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;
using NetTopologySuite.Geometries;

namespace PetCare.Domain.Aggregates
{
    public sealed class Shelter : BaseEntity
    {
        public Slug Slug { get; private set; }
        public Name Name { get; private set; }
        public Address Address { get; private set; }
        public Point Coordinates { get; private set; }
        public PhoneNumber ContactPhone { get; private set; }
        public Email ContactEmail { get; private set; }
        public string? Description { get; private set; }
        public int Capacity { get; private set; }
        public int CurrentOccupancy { get; private set; }
        public List<string> Photos { get; private set; } = new();
        public string? VirtualTourUrl { get; private set; }
        public string? WorkingHours { get; private set; }
        public Dictionary<string, string> SocialMedia { get; private set; } = new();
        public Guid? ManagerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set;}

        //EF Core навігація
        public User? Manager { get; private set; }

        private Shelter() { }

        private Shelter(
            Slug slug, 
            Name name, 
            Address address, 
            Point coordinates, 
            PhoneNumber contactPhone, 
            Email contactEmail, 
            string? description, 
            int capacity, 
            int currentOccupancy, 
            List<string> photos, 
            string? virtualTourUrl, 
            string? workingHours, 
            Dictionary<string, string> socialMedia, 
            Guid managerId)
        {
            Slug = slug;
            Name = name;
            Address = address;
            Coordinates = coordinates;
            ContactPhone = contactPhone;
            ContactEmail = contactEmail;
            Description = description;
            Capacity = capacity;
            CurrentOccupancy = currentOccupancy;
            Photos = photos;
            VirtualTourUrl = virtualTourUrl;
            WorkingHours = workingHours;
            SocialMedia = socialMedia;
            ManagerId = managerId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Shelter Create(
            string slug,
            string name,
            string address,
            Point coordinates,
            string contactPhone,
            string contactEmail,
            string? description,
            int capacity,
            int currentOccupancy,
            List<string>? photos,
            string? virtualTourUrl,
            string? workingHours,
            Dictionary<string, string>? socialMedia,
            Guid managerId)
        {
            return new Shelter(
                Slug.Create(slug),
                Name.Create(name),
                Address.Create(address),
                coordinates,
                PhoneNumber.Create(contactPhone),
                Email.Create(contactEmail),
                description,
                capacity,
                currentOccupancy,
                photos,
                virtualTourUrl,
                workingHours,
                socialMedia,
                managerId);
        }

        public void Update(
            string? name = null,
            string? address = null,
            Point? coordinates = null,
            string? contactPhone = null,
            string? contactEmail = null,
            string? description = null,
            int? capacity = null,
            int? currentOccupancy = null,
            List<string>? photos = null,
            string? virtualTourUrl = null,
            string? workingHours = null,
            Dictionary<string, string>? socialMedia = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = Name.Create(name);
            if (!string.IsNullOrWhiteSpace(address)) Address = Address.Create(address);
            if (coordinates != null) Coordinates = coordinates;
            if (!string.IsNullOrWhiteSpace(contactPhone)) ContactPhone = PhoneNumber.Create(contactPhone);
            if (!string.IsNullOrWhiteSpace(contactEmail)) ContactEmail = Email.Create(contactEmail);
            if (description != null) Description = description;
            if (capacity.HasValue) Capacity = capacity.Value;
            if (currentOccupancy.HasValue) CurrentOccupancy = currentOccupancy.Value;
            if (photos != null) Photos = photos;
            if (virtualTourUrl != null) VirtualTourUrl = virtualTourUrl;
            if (workingHours != null) WorkingHours = workingHours;
            if (socialMedia != null) SocialMedia = socialMedia;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
