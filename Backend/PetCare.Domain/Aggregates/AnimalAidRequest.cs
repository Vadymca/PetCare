// <copyright file="AnimalAidRequest.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a request for aid related to animals in the system.
/// </summary>
public sealed class AnimalAidRequest : BaseEntity
{
    private AnimalAidRequest()
    {
        this.Title = Title.Create(string.Empty);
    }

    private AnimalAidRequest(
        Guid? userId,
        Guid? shelterId,
        Title title,
        string? description,
        AidCategory category,
        AidStatus status,
        float? estimatedCost,
        List<string>? photos)
    {
        if (estimatedCost is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedCost), "Орієнтовна вартість має бути невід'ємною");
        }

        this.UserId = userId;
        this.ShelterId = shelterId;
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Status = status;
        this.EstimatedCost = estimatedCost;
        this.Photos = photos ?? new List<string>();
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique identifier of the user making the request, if any. Can be null.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter associated with the request, if any. Can be null.
    /// </summary>
    public Guid? ShelterId { get; private set; }

    /// <summary>
    /// Gets the title of the aid request.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the description of the aid request, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the category of the aid request.
    /// </summary>
    public AidCategory Category { get; private set; }

    /// <summary>
    /// Gets the current status of the aid request.
    /// </summary>
    public AidStatus Status { get; private set; }

    /// <summary>
    /// Gets the estimated cost of the aid request, if known. Can be null.
    /// </summary>
    public float? EstimatedCost { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the aid request.
    /// </summary>
    public List<string> Photos { get; private set; } = new();

    /// <summary>
    /// Gets the date and time when the aid request was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the aid request was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="AnimalAidRequest"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user making the request, if any. Can be null.</param>
    /// <param name="shelterId">The unique identifier of the shelter associated with the request, if any. Can be null.</param>
    /// <param name="title">The title of the aid request.</param>
    /// <param name="description">The description of the aid request, if any. Can be null.</param>
    /// <param name="category">The category of the aid request.</param>
    /// <param name="status">The current status of the aid request.</param>
    /// <param name="estimatedCost">The estimated cost of the aid request, if known. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the aid request. Can be null.</param>
    /// <returns>A new instance of <see cref="AnimalAidRequest"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="estimatedCost"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to <see cref="Title.Create"/>.</exception>
    public static AnimalAidRequest Create(
        Guid? userId,
        Guid? shelterId,
        string title,
        string? description,
        AidCategory category,
        AidStatus status,
        float? estimatedCost,
        List<string>? photos)
    {
        return new AnimalAidRequest(
            userId,
            shelterId,
            Title.Create(title),
            description,
            category,
            status,
            estimatedCost,
            photos);
    }

    /// <summary>
    /// Updates the status of the aid request.
    /// </summary>
    /// <param name="status">The new status of the aid request.</param>
    public void UpdateStatus(AidStatus status)
    {
        this.Status = status;
        this.UpdatedAt = DateTime.Now;
    }

    /// <summary>
    /// Updates the estimated cost of the aid request.
    /// </summary>
    /// <param name="newCost">The new estimated cost of the aid request. Can be null.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="newCost"/> is negative.</exception>
    public void UpdateEstimatedCost(float? newCost)
    {
        if (newCost is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newCost), "Вартість повинна бути невід'ємною.");
        }

        this.EstimatedCost = newCost;
        this.UpdatedAt = DateTime.UtcNow;
    }
}
