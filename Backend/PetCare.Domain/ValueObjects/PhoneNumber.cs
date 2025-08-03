// <copyright file="PhoneNumber.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a phone number as a value object, validated against the E.164 format.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex E164Regex = new(@"^\+[1-9]\d{6,14}$", RegexOptions.Compiled);

    private PhoneNumber(string value) => this.Value = value;

    /// <summary>
    /// Gets the phone number string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="PhoneNumber"/> instance after validating the E.164 format.
    /// </summary>
    /// <param name="phone">The phone number string to validate and encapsulate.</param>
    /// <returns>A new <see cref="PhoneNumber"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the phone number is null, empty, or invalid format.</exception>
    public static PhoneNumber Create(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Номер телефону не може бути порожнім.", nameof(phone));
        }

        phone = phone.Trim();

        if (!E164Regex.IsMatch(phone))
        {
            throw new ArgumentException(
                "Номер телефону повинен бути у дійсному форматі E.164 (наприклад, +380501112233).",
                nameof(phone));
        }

        return new PhoneNumber(phone);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
