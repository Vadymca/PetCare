// <copyright file="AnimalSubscription.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;

/// <summary>
/// Represents a value object that associates a user with a subscription to an animal.
/// </summary>
public sealed class AnimalSubscription : ValueObject
{
    private AnimalSubscription()
    {
    }

    private AnimalSubscription(Guid userId, Guid animalId, DateTime subscribedAt)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));
        }

        this.UserId = userId;
        this.AnimalId = animalId;
        this.SubscribedAt = subscribedAt;
    }

    /// <summary>
    /// Gets the unique identifier of the user subscribing to the animal.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal being subscribed to.
    /// </summary>
    public Guid AnimalId { get; private set; }

    /// <summary>
    /// Gets the date and time when the subscription was created.
    /// </summary>
    public DateTime SubscribedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="AnimalSubscription"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user subscribing to the animal.</param>
    /// <param name="animalId">The unique identifier of the animal being subscribed to.</param>
    /// <returns>A new instance of <see cref="AnimalSubscription"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> or <paramref name="animalId"/> is empty.</exception>
    public static AnimalSubscription Create(Guid userId, Guid animalId) =>
        new AnimalSubscription(userId, animalId, DateTime.UtcNow);

    /// <summary>
    /// Returns the components used to determine equality for the <see cref="AnimalSubscription"/> instance.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.UserId;
        yield return this.AnimalId;
    }
}
