// <copyright file="LostPet.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using NetTopologySuite.Geometries;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a lost pet record in the system.
/// </summary>
public sealed class LostPet : BaseEntity
{
    private readonly List<string> photos = new();

    private LostPet()
    {
        this.Slug = Slug.Create(string.Empty);
    }

    private LostPet(
        Slug slug,
        Guid userId,
        Guid? breedId,
        Name? name,
        string? description,
        Point? lastSeenLocation,
        DateTime? lastSeenDate,
        IEnumerable<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        decimal? reward,
        string? contactAlternative,
        MicrochipId? microchipId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        this.Slug = slug;
        this.UserId = userId;
        this.BreedId = breedId;
        this.Name = name;
        this.Description = description;
        this.LastSeenLocation = lastSeenLocation;
        this.LastSeenDate = lastSeenDate;
        if (photos != null)
        {
            this.photos.AddRange(photos);
        }

        if (reward is < 0)
        {
            throw new ArgumentException("Розмір винагороди не може бути від’ємним.", nameof(reward));
        }

        this.Status = status;
        this.AdminNotes = adminNotes;
        this.Reward = reward;
        this.ContactAlternative = contactAlternative;
        this.MicrochipId = microchipId;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the lost pet record.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the lost pet, if any. Can be null.
    /// </summary>
    public Name? Name { get; private set; }

    /// <summary>
    /// Gets the description of the lost pet, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the last known geographic location of the pet, if any. Can be null.
    /// </summary>
    public Point? LastSeenLocation { get; private set; }

    /// <summary>
    /// Gets the date and time when the pet was last seen, if any. Can be null.
    /// </summary>
    public DateTime? LastSeenDate { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the lost pet.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the current status of the lost pet record.
    /// </summary>
    public LostPetStatus Status { get; private set; }

    /// <summary>
    /// Gets the administrative notes for the lost pet record, if any. Can be null.
    /// </summary>
    public string? AdminNotes { get; private set; }

    /// <summary>
    /// Gets the reward offered for finding the pet, if any. Can be null.
    /// </summary>
    public decimal? Reward { get; private set; }

    /// <summary>
    /// Gets the alternative contact information for the lost pet, if any. Can be null.
    /// </summary>
    public string? ContactAlternative { get; private set; }

    /// <summary>
    /// Gets the microchip identifier of the pet, if any. Can be null.
    /// </summary>
    public MicrochipId? MicrochipId { get; private set; }

    /// <summary>
    /// Gets the date and time when the lost pet record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the lost pet record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user reporting the lost pet.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who reported the lost pet.
    /// Navigation property for EF Core.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the pet's breed, if any. Can be null.
    /// </summary>
    public Guid? BreedId { get; private set; }

    /// <summary>
    /// Gets the breed of the lost pet.
    /// Navigation property for EF Core.
    /// </summary>
    public Breed? Breed { get; private set; }

    /// <summary>
    /// Creates a new <see cref="LostPet"/> instance with the specified parameters.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the lost pet record.</param>
    /// <param name="userId">The unique identifier of the user reporting the lost pet.</param>
    /// <param name="breedId">The unique identifier of the pet's breed, if any. Can be null.</param>
    /// <param name="name">The name of the lost pet, if any. Can be null or empty.</param>
    /// <param name="description">The description of the lost pet, if any. Can be null.</param>
    /// <param name="lastSeenLocation">The last known geographic location of the pet, if any. Can be null.</param>
    /// <param name="lastSeenDate">The date and time when the pet was last seen, if any. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the lost pet, if any. Can be null.</param>
    /// <param name="status">The current status of the lost pet record.</param>
    /// <param name="adminNotes">The administrative notes for the lost pet record, if any. Can be null.</param>
    /// <param name="reward">The reward offered for finding the pet, if any. Can be null.</param>
    /// <param name="contactAlternative">The alternative contact information for the lost pet, if any. Can be null.</param>
    /// <param name="microchipId">The microchip identifier of the pet, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="LostPet"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static LostPet Create(
        string slug,
        Guid userId,
        Guid? breedId,
        string? name,
        string? description,
        Point? lastSeenLocation,
        DateTime? lastSeenDate,
        IEnumerable<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        decimal? reward,
        string? contactAlternative,
        string? microchipId)
    {
        return new LostPet(
            Slug.Create(slug),
            userId,
            breedId,
            string.IsNullOrWhiteSpace(name) ? null : Name.Create(name),
            description,
            lastSeenLocation,
            lastSeenDate,
            photos,
            status,
            adminNotes,
            reward,
            contactAlternative,
            microchipId is not null ? MicrochipId.Create(microchipId) : null);
    }

    /// <summary>
    /// Adds a photo URL to the photos collection.
    /// </summary>
    /// <param name="photoUrl">The URL of the photo to add.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="photoUrl"/> is null or whitespace.</exception>
    public void AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new ArgumentException("URL фотографії не може бути пустим.", nameof(photoUrl));
        }

        this.photos.Add(photoUrl);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a photo URL from the photos collection.
    /// </summary>
    /// <param name="photoUrl">The URL of the photo to remove.</param>
    public void RemovePhoto(string photoUrl)
    {
        if (this.photos.Remove(photoUrl))
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates the name of the lost pet.
    /// </summary>
    /// <param name="newName">The new name. Null or whitespace will clear the current name.</param>
    public void UpdateName(string? newName)
    {
        this.Name = string.IsNullOrWhiteSpace(newName) ? null : Name.Create(newName);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the description of the lost pet.
    /// </summary>
    /// <param name="newDescription">The new description. Can be null.</param>
    public void UpdateDescription(string? newDescription)
    {
        this.Description = newDescription;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the last seen location and date.
    /// </summary>
    /// <param name="location">The new last seen location. Can be null.</param>
    /// <param name="date">The new last seen date. Can be null.</param>
    public void UpdateLastSeen(Point? location, DateTime? date)
    {
        this.LastSeenLocation = location;
        this.LastSeenDate = date;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the alternative contact information.
    /// </summary>
    /// <param name="contact">The new alternative contact information. Can be null.</param>
    public void UpdateContactAlternative(string? contact)
    {
        this.ContactAlternative = contact;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the reward amount.
    /// </summary>
    /// <param name="rewardAmount">The new reward amount. Must not be negative.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="rewardAmount"/> is negative.</exception>
    public void UpdateReward(decimal? rewardAmount)
    {
        if (rewardAmount is < 0)
        {
            throw new ArgumentException("Розмір винагороди не може бути від’ємним.", nameof(rewardAmount));
        }

        this.Reward = rewardAmount;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the microchip ID.
    /// </summary>
    /// <param name="microchipId">The new microchip ID. Null to clear.</param>
    public void UpdateMicrochipId(string? microchipId)
    {
        this.MicrochipId = microchipId is null ? null : MicrochipId.Create(microchipId);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the status and optionally the administrative notes.
    /// </summary>
    /// <param name="status">The new status.</param>
    /// <param name="adminNotes">The new administrative notes. If null, notes remain unchanged.</param>
    public void UpdateStatus(LostPetStatus status, string? adminNotes = null)
    {
        this.Status = status;
        if (adminNotes != null)
        {
            this.AdminNotes = adminNotes;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assigns or updates the microchip ID for the lost pet.
    /// </summary>
    /// <param name="microchipId">The microchip ID to assign.</param>
    /// <exception cref="InvalidOperationException">Thrown when the microchip ID is already assigned.</exception>
    public void SetMicrochip(string microchipId)
    {
        var newMicrochip = MicrochipId.Create(microchipId);

        if (this.MicrochipId is not null && this.MicrochipId.Equals(newMicrochip))
        {
            throw new InvalidOperationException("Цей мікрочіп вже призначено цьому запису.");
        }

        this.MicrochipId = newMicrochip;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes the microchip ID from the lost pet.
    /// </summary>
    public void ClearMicrochip()
    {
        this.MicrochipId = null;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
