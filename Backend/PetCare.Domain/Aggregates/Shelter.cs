// <copyright file="Shelter.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using NetTopologySuite.Geometries;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a shelter in the system.
/// </summary>
public sealed class Shelter : BaseEntity
{
    private Shelter()
    {
        this.Slug = Slug.Create(string.Empty);
        this.Name = Name.Create(string.Empty);
        this.Address = Address.Create(string.Empty);
        this.Coordinates = new Point(0, 0) { SRID = 4326 };
        this.ContactPhone = PhoneNumber.Create(string.Empty);
        this.ContactEmail = Email.Create("default@petcare.com");
    }

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
        this.Slug = slug;
        this.Name = name;
        this.Address = address;
        this.Coordinates = coordinates;
        this.ContactPhone = contactPhone;
        this.ContactEmail = contactEmail;
        this.Description = description;
        this.Capacity = capacity;
        this.CurrentOccupancy = currentOccupancy;
        this.Photos = photos;
        this.VirtualTourUrl = virtualTourUrl;
        this.WorkingHours = workingHours;
        this.SocialMedia = socialMedia;
        this.ManagerId = managerId;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the shelter.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the shelter.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the address of the shelter.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Gets the geographic coordinates of the shelter.
    /// </summary>
    public Point Coordinates { get; private set; }

    /// <summary>
    /// Gets the contact phone number of the shelter.
    /// </summary>
    public PhoneNumber ContactPhone { get; private set; }

    /// <summary>
    /// Gets the contact email address of the shelter.
    /// </summary>
    public Email ContactEmail { get; private set; }

    /// <summary>
    /// Gets the description of the shelter, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the maximum capacity of the shelter.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Gets the current number of animals in the shelter.
    /// </summary>
    public int CurrentOccupancy { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the shelter.
    /// </summary>
    public List<string> Photos { get; private set; } = new();

    /// <summary>
    /// Gets the URL for the virtual tour of the shelter, if any. Can be null.
    /// </summary>
    public string? VirtualTourUrl { get; private set; }

    /// <summary>
    /// Gets the working hours of the shelter, if any. Can be null.
    /// </summary>
    public string? WorkingHours { get; private set; }

    /// <summary>
    /// Gets the social media links for the shelter.
    /// </summary>
    public Dictionary<string, string> SocialMedia { get; private set; } = new();

    /// <summary>
    /// Gets the date and time when the shelter record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the shelter record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter's manager, if any. Can be null.
    /// </summary>
    public Guid? ManagerId { get; private set; }

    /// <summary>
    /// Gets the manager associated with the shelter, if any. Can be null.
    /// </summary>
    public User? Manager { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Shelter"/> instance with the specified parameters.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the shelter.</param>
    /// <param name="name">The name of the shelter.</param>
    /// <param name="address">The address of the shelter.</param>
    /// <param name="coordinates">The geographic coordinates of the shelter.</param>
    /// <param name="contactPhone">The contact phone number of the shelter.</param>
    /// <param name="contactEmail">The contact email address of the shelter.</param>
    /// <param name="description">The description of the shelter, if any. Can be null.</param>
    /// <param name="capacity">The maximum capacity of the shelter.</param>
    /// <param name="currentOccupancy">The current number of animals in the shelter.</param>
    /// <param name="photos">The list of photo URLs for the shelter, if any. Can be null.</param>
    /// <param name="virtualTourUrl">The URL for the virtual tour of the shelter, if any. Can be null.</param>
    /// <param name="workingHours">The working hours of the shelter, if any. Can be null.</param>
    /// <param name="socialMedia">The social media links for the shelter, if any. Can be null.</param>
    /// <param name="managerId">The unique identifier of the shelter's manager, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Shelter"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, <paramref name="address"/>, <paramref name="contactPhone"/>, or <paramref name="contactEmail"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
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
            photos ?? new List<string>(),
            virtualTourUrl,
            workingHours,
            socialMedia ?? new Dictionary<string, string>(),
            managerId);
    }

    /// <summary>
    /// Updates the shelter's properties with the provided values.
    /// </summary>
    /// <param name="name">The new name of the shelter, if provided. If null or empty, the name remains unchanged.</param>
    /// <param name="address">The new address of the shelter, if provided. If null or empty, the address remains unchanged.</param>
    /// <param name="coordinates">The new geographic coordinates of the shelter, if provided. If null, the coordinates remain unchanged.</param>
    /// <param name="contactPhone">The new contact phone number of the shelter, if provided. If null or empty, the phone number remains unchanged.</param>
    /// <param name="contactEmail">The new contact email address of the shelter, if provided. If null or empty, the email remains unchanged.</param>
    /// <param name="description">The new description of the shelter, if provided. If null, the description remains unchanged.</param>
    /// <param name="capacity">The new maximum capacity of the shelter, if provided. If null, the capacity remains unchanged.</param>
    /// <param name="currentOccupancy">The new current number of animals in the shelter, if provided. If null, the occupancy remains unchanged.</param>
    /// <param name="photos">The new list of photo URLs for the shelter, if provided. If null, the photos remain unchanged.</param>
    /// <param name="virtualTourUrl">The new URL for the virtual tour of the shelter, if provided. If null, the virtual tour URL remains unchanged.</param>
    /// <param name="workingHours">The new working hours of the shelter, if provided. If null, the working hours remain unchanged.</param>
    /// <param name="socialMedia">The new social media links for the shelter, if provided. If null, the social media links remain unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/>, <paramref name="address"/>, <paramref name="contactPhone"/>, or <paramref name="contactEmail"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
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
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = Name.Create(name);
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            this.Address = Address.Create(address);
        }

        if (coordinates != null)
        {
            this.Coordinates = coordinates;
        }

        if (!string.IsNullOrWhiteSpace(contactPhone))
        {
            this.ContactPhone = PhoneNumber.Create(contactPhone);
        }

        if (!string.IsNullOrWhiteSpace(contactEmail))
        {
            this.ContactEmail = Email.Create(contactEmail);
        }

        if (description != null)
        {
            this.Description = description;
        }

        if (capacity.HasValue)
        {
            this.Capacity = capacity.Value;
        }

        if (currentOccupancy.HasValue)
        {
            this.CurrentOccupancy = currentOccupancy.Value;
        }

        if (photos != null)
        {
            this.Photos = photos;
        }

        if (virtualTourUrl != null)
        {
            this.VirtualTourUrl = virtualTourUrl;
        }

        if (workingHours != null)
        {
            this.WorkingHours = workingHours;
        }

        if (socialMedia != null)
        {
            this.SocialMedia = socialMedia;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }
}
