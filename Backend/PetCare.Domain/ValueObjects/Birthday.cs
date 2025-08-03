// <copyright file="Birthday.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;

/// <summary>
/// Represents a birthday as a value object.
/// </summary>
public sealed class Birthday : ValueObject
{
    private static readonly DateOnly MinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-90));

    private static readonly DateOnly MaxDate = DateOnly.FromDateTime(DateTime.UtcNow);

    private Birthday(DateOnly value) => this.Value = value;

    /// <summary>
    /// Gets the value of the birthday.
    /// </summary>
    public DateOnly Value { get; }

    /// <summary>
    /// Creates a new <see cref="Birthday"/> instance after validating the input.
    /// </summary>
    /// <param name="date">The birthday date.</param>
    /// <returns>A new instance of <see cref="Birthday"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date is in the future or unreasonably far in the past.</exception>
    public static Birthday Create(DateOnly date)
    {
        if (date > MaxDate)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Дата народження не може бути в майбутньому.");
        }

        if (date < MinDate)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Дата народження надто стара для системи.");
        }

        return new Birthday(date);
    }

    /// <summary>
    /// Checks if the provided date is valid as a birthday.
    /// </summary>
    /// <param name="date">The birthday date to validate.</param>
    /// <returns>True if the date is within valid bounds; otherwise, false.</returns>
    public static bool IsValid(DateOnly date) =>
        date <= MaxDate && date >= MinDate;

    /// <inheritdoc/>
    public override string ToString() => this.Value.ToString("yyyy-MM-dd");

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new object[] { this.Value };
}
