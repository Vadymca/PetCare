// <copyright file="Specie.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a species of animal in the system.
/// </summary>
public sealed class Specie : BaseEntity
{
    private Specie()
    {
        this.Name = Name.Create(string.Empty);
    }

    private Specie(Name name)
    {
        this.Name = name;
        this.Breeds = new List<Breed>();
    }

    /// <summary>
    /// Gets the name of the species.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the collection of breeds associated with the species.
    /// </summary>
    public ICollection<Breed> Breeds { get; private set; } = new List<Breed>();

    /// <summary>
    /// Creates a new <see cref="Specie"/> instance with the specified name.
    /// </summary>
    /// <param name="name">The name of the species.</param>
    /// <returns>A new instance of <see cref="Specie"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public static Specie Create(string name)
    {
        return new Specie(Name.Create(name));
    }

    /// <summary>
    /// Updates the name of the species.
    /// </summary>
    /// <param name="newName">The new name for the species.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newName"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public void Rename(string newName)
    {
        this.Name = Name.Create(newName);
    }
}
