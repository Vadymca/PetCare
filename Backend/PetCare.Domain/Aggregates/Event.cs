// <copyright file="Event.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an event in the system.
/// </summary>
public sealed class Event : BaseEntity
{
    private Event()
    {
        this.Title = Title.Create(string.Empty);
    }

    private Event(
        Guid? shelterId,
        Title title,
        string? description,
        DateTime? eventDate,
        ValueObjects.Coordinates? location,
        Address? address,
        EventType type,
        EventStatus status)
    {
        if (eventDate is not null && eventDate <= DateTime.UtcNow)
        {
            throw new ArgumentException("Дата події повинна бути в майбутньому.", nameof(eventDate));
        }

        this.ShelterId = shelterId;
        this.Title = title ?? throw new ArgumentNullException(nameof(title));
        this.Description = description;
        this.EventDate = eventDate;
        this.Location = location;
        this.Address = address;
        this.Type = type;
        this.Status = status;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the title of the event.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the description of the event, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the date and time of the event, if specified. Can be null.
    /// </summary>
    public DateTime? EventDate { get; private set; }

    /// <summary>
    /// Gets the geographic location of the event, if specified. Can be null.
    /// </summary>
    public ValueObjects.Coordinates? Location { get; private set; }

    /// <summary>
    /// Gets the address of the event, if specified. Can be null.
    /// </summary>
    public Address? Address { get; private set; }

    /// <summary>
    /// Gets the type of the event.
    /// </summary>
    public EventType Type { get; private set; }

    /// <summary>
    /// Gets the current status of the event.
    /// </summary>
    public EventStatus Status { get; private set; }

    /// <summary>
    /// Gets the date and time when the event was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the event was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter associated with the event, if any. Can be null.
    /// </summary>
    public Guid? ShelterId { get; private set; }

    /// <summary>
    /// Gets the shelter associated with this event, if any. Can be null.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Event"/> instance with the specified parameters.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter associated with the event, if any. Can be null.</param>
    /// <param name="title">The title of the event.</param>
    /// <param name="description">The description of the event, if any. Can be null.</param>
    /// <param name="eventDate">The date and time of the event, if specified. Can be null.</param>
    /// <param name="location">The geographic location of the event, if specified. Can be null.</param>
    /// <param name="address">The address of the event, if specified. Can be null.</param>
    /// <param name="type">The type of the event.</param>
    /// <param name="status">The current status of the event.</param>
    /// <returns>A new instance of <see cref="Event"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="address"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static Event Create(
        Guid? shelterId,
        string title,
        string? description,
        DateTime? eventDate,
        ValueObjects.Coordinates? location,
        string? address,
        EventType type,
        EventStatus status)
    {
        return new Event(
            shelterId,
            Title.Create(title),
            description,
            eventDate,
            location,
            address is not null ? Address.Create(address) : null,
            type,
            status);
    }

    /// <summary>
    /// Updates the properties of the event, if provided.
    /// </summary>
    /// <param name="title">The new title of the event, if provided. If null or empty, the title remains unchanged.</param>
    /// <param name="description">The new description of the event, if provided. If null, the description remains unchanged.</param>
    /// <param name="eventDate">The new date and time of the event, if provided. If null, the event date remains unchanged.</param>
    /// <param name="location">The new geographic location of the event, if provided. If null, the location remains unchanged.</param>
    /// <param name="address">The new address of the event, if provided. If null, the address remains unchanged.</param>
    /// <param name="status">The new status of the event, if provided. If null, the status remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="address"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public void Update(
        string? title = null,
        string? description = null,
        DateTime? eventDate = null,
        ValueObjects.Coordinates? location = null,
        string? address = null,
        EventStatus? status = null)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            this.Title = Title.Create(title);
        }

        if (description is not null)
        {
            this.Description = description;
        }

        if (eventDate is not null)
        {
            this.EventDate = eventDate;
        }

        if (location is not null)
        {
            this.Location = location;
        }

        if (address is not null)
        {
            this.Address = Address.Create(address);
        }

        if (status is not null)
        {
            this.Status = status.Value;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the event.
    /// </summary>
    public void Cancel()
    {
        if (this.Status == EventStatus.Cancelled)
        {
            throw new InvalidOperationException("Подія вже скасована.");
        }

        this.Status = EventStatus.Cancelled;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Completes the event.
    /// </summary>
    public void Complete()
    {
        if (this.Status == EventStatus.Completed)
        {
            throw new InvalidOperationException("Подія вже завершена.");
        }

        this.Status = EventStatus.Completed;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reschedules the event by updating its planned date.
    /// </summary>
    /// <param name="newDate">The future date to which the event is postponed.</param>
    /// <exception cref="ArgumentException">Thrown if the provided date is not in the future.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the event has already been completed or cancelled.</exception>
    public void Postpone(DateTime newDate)
    {
        if (newDate <= DateTime.UtcNow)
        {
            throw new ArgumentException("Нова дата повинна бути в майбутньому.", nameof(newDate));
        }

        this.EventDate = newDate;

        if (this.Status is EventStatus.Completed or EventStatus.Cancelled)
        {
            throw new InvalidOperationException("Не можна перенести завершену або скасовану подію.");
        }

        this.Status = EventStatus.Planned;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the location coordinates of the event.
    /// </summary>
    /// <param name="coordinates">The new geographical coordinates of the event.</param>
    public void UpdateCoordinates(Coordinates coordinates)
    {
        this.Location = coordinates;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the address of the event.
    /// </summary>
    /// <param name="address">The new address of the event as a single-line string.</param>
    public void UpdateAddress(string address)
    {
        this.Address = Address.Create(address);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Assigns the event to a specific shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter.</param>
    public void AssignToShelter(Guid shelterId)
    {
        this.ShelterId = shelterId;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
