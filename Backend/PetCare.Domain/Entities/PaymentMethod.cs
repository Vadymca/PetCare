// <copyright file="PaymentMethod.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a payment method used in the system.
/// </summary>
public sealed class PaymentMethod : BaseEntity
{
    private PaymentMethod()
    {
        this.Name = Name.Create(string.Empty);
    }

    private PaymentMethod(Name name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the name of the payment method.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Creates a new <see cref="PaymentMethod"/> instance with the specified name.
    /// </summary>
    /// <param name="name">The name of the payment method.</param>
    /// <returns>A new instance of <see cref="PaymentMethod"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public static PaymentMethod Create(Name name)
    {
        return new PaymentMethod(name);
    }

    /// <summary>
    /// Updates the name of the payment method.
    /// </summary>
    /// <param name="newName">The new name for the payment method.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newName"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public void Rename(string newName)
    {
        this.Name = Name.Create(newName);
    }
}
