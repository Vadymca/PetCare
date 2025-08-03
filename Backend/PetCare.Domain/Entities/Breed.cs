// <copyright file="Breed.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a breed of an animal, associated with a specific species.
/// </summary>
public sealed class Breed : BaseEntity
{
    private Breed()
    {
        this.SpeciesId = Guid.Empty;
        this.Name = Name.Create(string.Empty);
    }

    private Breed(Guid speciesId, Name name, string? description)
    {
        this.SpeciesId = speciesId;
        this.Name = name;
        this.Description = description;
    }

    /// <summary>
    /// Gets the name of the breed.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the description of the breed, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the species the breed belongs to.
    /// </summary>
    public Guid SpeciesId { get; private set; }

    /// <summary>
    /// Gets the associated species navigation property for Entity Framework Core.
    /// </summary>
    public Specie? Specie { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Breed"/> instance with the specified parameters.
    /// </summary>
    /// <param name="speciesId">The unique identifier of the species the breed belongs to.</param>
    /// <param name="name">The name of the breed.</param>
    /// <param name="description">The description of the breed, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Breed"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="speciesId"/> is empty or when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public static Breed Create(Guid speciesId, string name, string? description)
    {
        if (speciesId == Guid.Empty)
        {
            throw new ArgumentException("Обов'язкова ідентифікація виду.", nameof(speciesId));
        }

        return new Breed(
            speciesId,
            Name.Create(name),
            description);
    }

    /// <summary>
    /// Updates the breed's name or description, if provided.
    /// </summary>
    /// <param name="name">The new name of the breed, if provided. If null, the name remains unchanged.</param>
    /// <param name="description">The new description of the breed, if provided. If null, the description remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public void Update(
        string? name = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = Name.Create(name);
        }

        if (description is not null)
        {
            this.Description = description;
        }
    }
}
