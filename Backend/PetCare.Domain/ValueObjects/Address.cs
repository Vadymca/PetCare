// <copyright file="Address.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a postal address as a value object with validation.
/// </summary>
public sealed class Address : ValueObject
{
    private static readonly Regex AddressRegex =
        new(@"^(вул\.|пров\.|просп\.|пл\.|бульв\.)\s+\p{L}+.*?,\s*(№?\s*\d+[A-Za-zА-Яа-я]?),\s*м\.\s*\p{L}+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Address(string value) => this.Value = value;

    /// <summary>
    /// Gets the address string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Address"/> instance after validating the input string.
    /// </summary>
    /// <param name="address">The address string to validate and encapsulate.</param>
    /// <returns>A new <see cref="Address"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the address is null, empty, or invalid format.</exception>
    public static Address Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Адреса не може бути порожньою.", nameof(address));
        }

        var trimmed = address.Trim();

        if (trimmed.Length < 10 || trimmed.Length > 200)
        {
            throw new ArgumentException("Адреса повинна містити від 10 до 200 символів.", nameof(address));
        }

        if (!AddressRegex.IsMatch(trimmed))
        {
            throw new ArgumentException("Формат адреси невірний. Приклад: 'вул. Головна, 12, м. Чернівці'", nameof(address));
        }

        return new Address(trimmed);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
