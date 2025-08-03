// <copyright file="Title.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;

/// <summary>
/// Represents a title as a value object with validation.
/// </summary>
public sealed class Title : ValueObject
{
    private const int MaxLength = 255;

    private Title(string value) => this.Value = value;

    /// <summary>
    /// Gets the title string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Title"/> instance after validating the input.
    /// </summary>
    /// <param name="title">The title string.</param>
    /// <returns>A new <see cref="Title"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the title is null, empty, or exceeds the maximum allowed length.
    /// </exception>
    public static Title Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Заголовок не може бути порожнім.", nameof(title));
        }

        if (title.Length > MaxLength)
        {
            throw new ArgumentException($"Назва не може бути довшою за {MaxLength} символів.", nameof(title));
        }

        return new Title(title);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
