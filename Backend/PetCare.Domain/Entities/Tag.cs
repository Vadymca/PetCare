// <copyright file="Tag.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;

/// <summary>
/// Represents a tag used for categorizing or labeling content in the system.
/// </summary>
public sealed class Tag : BaseEntity
{
    private Tag()
    {
    }

    private Tag(string name, string? icon)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ім'я не може бути порожнім.", nameof(name));
        }

        this.Name = name.Trim();
        this.Icon = string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
        this.CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the name of the tag.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// Gets the icon associated with the tag, if any. Can be null.
    /// </summary>
    public string? Icon { get; private set; }

    /// <summary>
    /// Gets the date and time when the tag was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Tag"/> instance with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the tag.</param>
    /// <param name="icon">The icon associated with the tag, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Tag"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    public static Tag Create(string name, string? icon = null)
    {
        return new Tag(name, icon);
    }

    /// <summary>
    /// Updates the tag's name or icon, if provided.
    /// </summary>
    /// <param name="name">The new name of the tag, if provided. If null or whitespace, the name remains unchanged.</param>
    /// <param name="icon">The new icon for the tag, if provided. If null, the icon remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    public void Update(string? name = null, string? icon = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = name.Trim();
        }

        this.Icon = icon?.Trim();
    }
}
