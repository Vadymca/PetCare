// <copyright file="Animal.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an animal in the system.
/// </summary>
public sealed class Animal : BaseEntity
{
    private readonly List<string> photos = new();
    private readonly List<string> videos = new();

    private Animal()
    {
        this.Slug = Slug.Create(string.Empty);
        this.Name = Name.Create(string.Empty);
    }

    private Animal(
        Slug slug,
        Guid userId,
        Name name,
        Guid breedId,
        Birthday? birthday,
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
        this.Slug = slug;
        this.UserId = userId != Guid.Empty
            ? userId
            : throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        this.Name = name;
        this.BreedId = breedId != Guid.Empty
           ? breedId
           : throw new ArgumentException("Ідентифікатор породи не може бути порожнім.", nameof(breedId));
        this.Birthday = birthday;
        this.Gender = gender;
        this.Description = description;
        this.HealthStatus = healthStatus;
        this.photos = photos ?? new List<string>();
        this.videos = videos ?? new List<string>();
        this.ShelterId = shelterId != Guid.Empty
           ? shelterId
           : throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        this.Status = status;
        this.AdoptionRequirements = adoptionRequirements;
        this.MicrochipId = microchipId;
        this.IdNumber = idNumber;
        this.Weight = weight;
        this.Height = height;
        this.Color = color;
        this.IsSterilized = isSterilized;
        this.HaveDocuments = haveDocuments;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the animal.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the animal.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the birthday of the animal, if known. Can be null.
    /// </summary>
    public Birthday? Birthday { get; private set; }

    /// <summary>
    /// Gets the gender of the animal.
    /// </summary>
    public AnimalGender Gender { get; private set; }

    /// <summary>
    /// Gets the description of the animal, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the health status of the animal, if any. Can be null.
    /// </summary>
    public string? HealthStatus { get; private set; }

    /// <summary>
    /// Gets the photos of the animal.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the videos of the animal.
    /// </summary>
    public IReadOnlyList<string> Videos => this.videos.AsReadOnly();

    /// <summary>
    /// Gets the current status of the animal.
    /// </summary>
    public AnimalStatus Status { get; private set; }

    /// <summary>
    /// Gets the adoption requirements for the animal, if any. Can be null.
    /// </summary>
    public string? AdoptionRequirements { get; private set; }

    /// <summary>
    /// Gets the microchip identifier for the animal, if any. Can be null.
    /// </summary>
    public MicrochipId? MicrochipId { get; private set; }

    /// <summary>
    /// Gets the identification number of the animal.
    /// </summary>
    public int IdNumber { get; private set; }

    /// <summary>
    /// Gets the weight of the animal in kilograms, if known. Can be null.
    /// </summary>
    public float? Weight { get; private set; }

    /// <summary>
    /// Gets the height of the animal in centimeters, if known. Can be null.
    /// </summary>
    public float? Height { get; private set; }

    /// <summary>
    /// Gets the color of the animal, if any. Can be null.
    /// </summary>
    public string? Color { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal is sterilized.
    /// </summary>
    public bool IsSterilized { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal has documents.
    /// </summary>
    public bool HaveDocuments { get; private set; }

    /// <summary>
    /// Gets the date and time when the animal record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the animal record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user associated with the animal.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user associated with the animal, if any. Can be null.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal's breed.
    /// </summary>
    public Guid BreedId { get; private set; }

    /// <summary>
    /// Gets the breed of the animal, if any. Can be null.
    /// </summary>
    public Breed? Breed { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter hosting the animal.
    /// </summary>
    public Guid ShelterId { get; private set; }

    /// <summary>
    /// Gets the shelter hosting the animal, if any. Can be null.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal is eligible for adoption.
    /// </summary>
    public bool CanBeAdopted => this.Status == AnimalStatus.Available;

    /// <summary>
    /// Gets a value indicating whether the animal has a microchip.
    /// </summary>
    public bool HasMicrochip => this.MicrochipId is not null;

    /// <summary>
    /// Creates a new <see cref="Animal"/> instance with the specified parameters.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the animal.</param>
    /// <param name="userId">The unique identifier of the user associated with the animal.</param>
    /// <param name="name">The name of the animal.</param>
    /// <param name="breedId">The unique identifier of the animal's breed.</param>
    /// <param name="birthday">The birthday of the animal, if known. Can be null.</param>
    /// <param name="gender">The gender of the animal.</param>
    /// <param name="description">The description of the animal, if any. Can be null.</param>
    /// <param name="healthStatus">The health status of the animal, if any. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the animal. Can be null.</param>
    /// <param name="videos">The list of video URLs for the animal. Can be null.</param>
    /// <param name="shelterId">The unique identifier of the shelter hosting the animal.</param>
    /// <param name="status">The current status of the animal.</param>
    /// <param name="adoptionRequirements">The adoption requirements for the animal, if any. Can be null.</param>
    /// <param name="microchipId">The microchip identifier for the animal, if any. Can be null.</param>
    /// <param name="idNumber">The identification number of the animal.</param>
    /// <param name="weight">The weight of the animal in kilograms, if known. Can be null.</param>
    /// <param name="height">The height of the animal in centimeters, if known. Can be null.</param>
    /// <param name="color">The color of the animal, if any. Can be null.</param>
    /// <param name="isSterilized">Indicates whether the animal is sterilized.</param>
    /// <param name="haveDocuments">Indicates whether the animal has documents.</param>
    /// <returns>A new instance of <see cref="Animal"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static Animal Create(
        string slug,
        Guid userId,
        string name,
        Guid breedId,
        Birthday? birthday,
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
            birthday is not null ? Birthday.Create(birthday.Value) : null,
            gender,
            description,
            healthStatus,
            photos ?? new(),
            videos ?? new(),
            shelterId,
            status,
            adoptionRequirements,
            microchipId is not null ? MicrochipId.Create(microchipId) : null,
            idNumber,
            weight,
            height,
            color,
            isSterilized,
            haveDocuments);
    }

    /// <summary>
    /// Updates the properties of the animal, if provided.
    /// </summary>
    /// <param name="name">The new name of the animal, if provided. If null, the name remains unchanged.</param>
    /// <param name="birthday">The new birthday of the animal, if provided. If null, the birthday remains unchanged.</param>
    /// <param name="gender">The new gender of the animal, if provided. If null, the gender remains unchanged.</param>
    /// <param name="description">The new description of the animal, if provided. If null, the description remains unchanged.</param>
    /// <param name="healthStatus">The new health status of the animal, if provided. If null, the health status remains unchanged.</param>
    /// <param name="status">The new status of the animal, if provided. If null, the status remains unchanged.</param>
    /// <param name="adoptionRequirements">The new adoption requirements for the animal, if provided. If null, the adoption requirements remain unchanged.</param>
    /// <param name="microchipId">The new microchip identifier for the animal, if provided. If null, the microchip identifier remains unchanged.</param>
    /// <param name="weight">The new weight of the animal in kilograms, if provided. If null, the weight remains unchanged.</param>
    /// <param name="height">The new height of the animal in centimeters, if provided. If null, the height remains unchanged.</param>
    /// <param name="color">The new color of the animal, if provided. If null, the color remains unchanged.</param>
    /// <param name="isSterilized">Indicates whether the animal is sterilized, if provided. If null, the sterilization status remains unchanged.</param>
    /// <param name="haveDocuments">Indicates whether the animal has documents, if provided. If null, the document status remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public void Update(
        string? name = null,
        Birthday? birthday = null,
        AnimalGender? gender = null,
        string? description = null,
        string? healthStatus = null,
        AnimalStatus? status = null,
        string? adoptionRequirements = null,
        string? microchipId = null,
        float? weight = null,
        float? height = null,
        string? color = null,
        bool? isSterilized = null,
        bool? haveDocuments = null)
    {
        if (name is not null)
        {
            this.Name = Name.Create(name);
        }

        if (birthday is not null)
        {
            this.Birthday = birthday;
        }

        if (gender is not null)
        {
            this.Gender = gender.Value;
        }

        if (description is not null)
        {
            this.Description = description;
        }

        if (healthStatus is not null)
        {
            this.HealthStatus = healthStatus;
        }

        if (status is not null)
        {
            this.Status = status.Value;
        }

        if (adoptionRequirements is not null)
        {
            this.AdoptionRequirements = adoptionRequirements;
        }

        if (microchipId is not null)
        {
            this.MicrochipId = MicrochipId.Create(microchipId);
        }

        if (weight is not null)
        {
            this.Weight = weight;
        }

        if (height is not null)
        {
            this.Height = height;
        }

        if (color is not null)
        {
            this.Color = color;
        }

        if (isSterilized is not null)
        {
            this.IsSterilized = isSterilized.Value;
        }

        if (haveDocuments is not null)
        {
            this.HaveDocuments = haveDocuments.Value;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Changes the status of the animal.
    /// </summary>
    /// /// <param name="newStatus">The new status to apply.</param>
    public void ChangeStatus(AnimalStatus newStatus)
    {
        this.Status = newStatus;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a photo to the animal.
    /// </summary>
    /// <param name="url">The URL of the photo to add.</param>
    public void AddPhoto(string url)
    {
        if (!string.IsNullOrWhiteSpace(url) && !this.photos.Contains(url))
        {
            this.photos.Add(url);
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Removes a photo from the animal.
    /// </summary>
    /// <param name="url">The URL of the photo to remove.</param>
    public void RemovePhoto(string url)
    {
        if (this.photos.Remove(url))
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Adds a video to the animal.
    /// </summary>
    /// <param name="url">The URL of the video to add.</param>
    public void AddVideo(string url)
    {
        if (!string.IsNullOrWhiteSpace(url) && !this.videos.Contains(url))
        {
            this.videos.Add(url);
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Removes a video from the animal.
    /// </summary>
    /// <param name="url">The URL of the video to remove.</param>
    public void RemoveVideo(string url)
    {
        if (this.videos.Remove(url))
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Validates whether the animal has sufficient adoption requirements defined.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the animal's adoption requirements are missing or too short.
    /// </exception>
    public void ValidateAdoptionRequirements()
    {
        if (string.IsNullOrWhiteSpace(this.AdoptionRequirements) || this.AdoptionRequirements.Length < 10)
        {
            throw new InvalidOperationException("Вимоги до адопції тварини не заповнені або занадто короткі.");
        }
    }
}
