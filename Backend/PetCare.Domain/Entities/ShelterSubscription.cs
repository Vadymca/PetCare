// <copyright file="ShelterSubscription.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;

/// <summary>
/// Represents a value object that associates a user with a shelter subscription.
/// </summary>
public sealed class ShelterSubscription : ValueObject
{
    private ShelterSubscription()
    {
    }

    private ShelterSubscription(Guid userId, Guid shelterId, DateTime subscribedAt)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (shelterId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        }

        this.UserId = userId;
        this.ShelterId = shelterId;
        this.SubscribedAt = subscribedAt;
    }

    /// <summary>
    /// Gets the unique identifier of the user subscribing to the shelter.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who is subscribed to the shelter.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter.
    /// </summary>
    public Guid ShelterId { get; private set; }

    /// <summary>
    /// Gets the shelter to which the user is subscribed.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Gets the date and time when the user subscribed to the shelter.
    /// </summary>
    public DateTime SubscribedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="ShelterSubscription"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user subscribing to the shelter.</param>
    /// <param name="shelterId">The unique identifier of the shelter.</param>
    /// <returns>A new instance of <see cref="ShelterSubscription"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> or <paramref name="shelterId"/> is empty.</exception>
    public static ShelterSubscription Create(Guid userId, Guid shelterId) =>
        new ShelterSubscription(userId, shelterId, DateTime.UtcNow);

    /// <summary>
    /// Returns the components used to determine equality for the <see cref="ShelterSubscription"/> instance.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.UserId;
        yield return this.ShelterId;
    }
}
