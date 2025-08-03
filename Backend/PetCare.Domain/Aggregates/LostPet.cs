// <copyright file="LostPet.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using NetTopologySuite.Geometries;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a lost pet record in the system.
/// </summary>
public sealed class LostPet : BaseEntity
{
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
        List<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        float? reward,
        string? contactAlternative,
        MicrochipId? microchipId)
    {
        this.Slug = slug;
        this.UserId = userId;
        this.BreedId = breedId;
        this.Name = name;
        this.Description = description;
        this.LastSeenLocation = lastSeenLocation;
        this.LastSeenDate = lastSeenDate;
        this.Photos = photos ?? new List<string>();
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
    /// Gets the unique identifier of the user reporting the lost pet.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the pet's breed, if any. Can be null.
    /// </summary>
    public Guid? BreedId { get; private set; }

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
    public List<string> Photos { get; private set; } = new();

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
    public float? Reward { get; private set; }

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
        List<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        float? reward,
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
    /// Updates the status and optionally the administrative notes of the lost pet record.
    /// </summary>
    /// <param name="status">The new status of the lost pet record.</param>
    /// <param name="adminNotes">The new administrative notes, if provided. If null, the notes remain unchanged.</param>
    public void UpdateStatus(LostPetStatus status, string? adminNotes = null)
    {
        this.Status = status;
        if (adminNotes != null)
        {
            this.AdminNotes = adminNotes;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }
}
