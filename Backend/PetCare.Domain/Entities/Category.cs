// <copyright file="Category.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a category for organizing content, such as articles or resources.
/// </summary>
public sealed class Category : BaseEntity
{
    private Category()
    {
        this.Name = Name.Create(string.Empty);
        this.CreatedAt = DateTime.UtcNow;
    }

    private Category(Name name, string? description)
    {
        this.Name = name;
        this.Description = description;
        this.CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the name of the category.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the description of the category, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the date and time when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Category"/> instance with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the category.</param>
    /// <param name="description">The description of the category, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Category"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public static Category Create(string name, string? description = null)
    {
        return new Category(Name.Create(name), description);
    }

    /// <summary>
    /// Updates the category's name or description, if provided.
    /// </summary>
    /// <param name="name">The new name of the category, if provided. If null, the name remains unchanged.</param>
    /// <param name="description">The new description of the category, if provided. If null, the description remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public void Update(string? name = null, string? description = null)
    {
        if (name is not null)
        {
            this.Name = Name.Create(name);
        }

        if (description is not null)
        {
            this.Description = description;
        }
    }
}
