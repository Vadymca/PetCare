using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

namespace PetCare.Domain.Aggregates
{
    public sealed class Animal : BaseEntity
    {
        public Slug Slug { get; private set; }
        public Guid UserId { get; private set; }
        public Name Name { get; private set; }
        public Guid BreedId { get; private set; }
        public DateTime? Birthday { get; private set; }
        public AnimalGender Gender { get; private set; }
        public string? Description { get; private set; }
        public string? HealthStatus { get; private set; }
        public List<string> Photos { get; private set; } = new();
        public List<string> Videos { get; private set; } = new();
        public Guid ShelterId { get; private set; }
        public AnimalStatus Status { get; private set; }
        public string? AdoptionRequirements { get; private set; }
        public MicrochipId? MicrochipId { get; private set; }
        public int IdNumber { get; private set; }
        public float? Weight { get; private set; }
        public float? Height { get; private set; }
        public string? Color { get; private set; }
        public bool IsSterilized { get; private set; }
        public bool HaveDocuments { get; private set; }
        public DateTime CreatedAt {  get; private set; }
        public DateTime UpdatedAt { get; private set; }

        //EF Core навігація
        public User? User { get; private set; }
        public Breed? Breed { get; private set; }
        public Shelter? Shelter { get; private set; }

        private Animal() { }

        private Animal(
            Slug slug, 
            Guid userId, 
            Name name, 
            Guid breedId, 
            DateTime? birthday, 
            AnimalGender gender, 
            string? description, 
            string? healthStatus, 
            List<string> photos, 
            List<string> videos, 
            Guid shelterId, 
            AnimalStatus status, 
            string? adoptionRequirements, 
            MicrochipId? microchipId, 
            int idNumber, 
            float? weight, 
            float? height, 
            string? color, 
            bool isSterilized, 
            bool haveDocuments)
        {
            Slug = slug;
            UserId = userId;
            Name = name;
            BreedId = breedId;
            Birthday = birthday;
            Gender = gender;
            Description = description;
            HealthStatus = healthStatus;
            Photos = photos;
            Videos = videos;
            ShelterId = shelterId;
            Status = status;
            AdoptionRequirements = adoptionRequirements;
            MicrochipId = microchipId;
            IdNumber = idNumber;
            Weight = weight;
            Height = height;
            Color = color;
            IsSterilized = isSterilized;
            HaveDocuments = haveDocuments;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
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
                weight,
                height,
                color,
                isSterilized,
                haveDocuments
            );
        }

        public void Update(
            string? name = null,
            DateTime? birthday = null,
            AnimalGender? gender = null,
            string? description = null,
            string? healthStatus = null,
            List<string>? photos = null,
            List<string>? videos = null,
            AnimalStatus? status = null,
            string? adoptionRequirements = null,
            string? microchipId = null,
            float? weight = null,
            float? height = null,
            string? color = null,
            bool? isSterilized = null,
            bool? haveDocuments = null)
        {
            if (name is not null) Name = Name.Create(name);
            if (birthday is not null) Birthday = birthday;
            if (gender is not null) Gender = gender.Value;
            if (description is not null) Description = description;
            if (healthStatus is not null) HealthStatus = healthStatus;
            if (photos is not null) Photos = photos;
            if (videos is not null) Videos = videos;
            if (status is not null) Status = status.Value;
            if (adoptionRequirements is not null) AdoptionRequirements = adoptionRequirements;
            if (microchipId is not null) MicrochipId = MicrochipId.Create(microchipId);
            if (weight is not null) Weight = weight;
            if (height is not null) Height = height;
            if (color is not null) Color = color;
            if (isSterilized is not null) IsSterilized = isSterilized.Value;
            if (haveDocuments is not null) HaveDocuments = haveDocuments.Value;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
