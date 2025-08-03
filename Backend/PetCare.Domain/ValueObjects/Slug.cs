// <copyright file="Slug.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a URL-friendly slug as a value object with validation and normalization.
/// </summary>
public sealed class Slug : ValueObject
{
    private static readonly Regex SlugRegex = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);

    private Slug(string value) => this.Value = value;

    /// <summary>
    /// Gets the slug string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Slug"/> instance after normalizing and validating the input.
    /// </summary>
    /// <param name="slug">The input string to normalize and validate as a slug.</param>
    /// <returns>A new <see cref="Slug"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the slug is null, empty, or invalid after normalization.
    /// </exception>
    public static Slug Create(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        var normalized = slug.Trim().ToLowerInvariant();

        normalized = Regex.Replace(normalized, @"[\s_]+", "-");

        normalized = Regex.Replace(normalized, @"[^a-z0-9\-]", string.Empty);

        normalized = Regex.Replace(normalized, @"-+", "-");

        normalized = normalized.Trim('-');

        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ArgumentException("Slug не містить допустимих символів після форматування.", nameof(slug));
        }

        if (!SlugRegex.IsMatch(normalized))
        {
            throw new ArgumentException("Slug не дійсний.", nameof(slug));
        }

        return new Slug(normalized);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
