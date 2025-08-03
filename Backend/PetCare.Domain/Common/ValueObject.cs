// <copyright file="ValueObject.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Common;

/// <summary>
/// Represents an abstract base class for value objects, providing equality comparison based on components.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object. Can be null.</param>
    /// <returns><c>true</c> if the specified object is a value object with the same equality components; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) =>
        obj is ValueObject other && this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    /// <summary>
    /// Generates a hash code based on the equality components of the value object.
    /// </summary>
    /// <returns>A hash code for the current value object.</returns>
    public override int GetHashCode() =>
        this.GetEqualityComponents().Aggregate(1, (hash, obj) => HashCode.Combine(hash, obj));

    /// <summary>
    /// Gets the components used for equality comparison.
    /// </summary>
    /// <returns>An enumerable collection of objects representing the equality components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();
}
