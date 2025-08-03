// <copyright file="Email.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Represents an email address as a value object with validation.
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(@"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,4}$", RegexOptions.Compiled);

    private Email(string value) => this.Value = value;

    /// <summary>
    /// Gets the email address string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Email"/> instance after validation and normalization.
    /// </summary>
    /// <param name="email">The input email string.</param>
    /// <returns>A validated and normalized <see cref="Email"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the email is null or invalid.</exception>
    public static Email Create(string email)
    {
        var normalized = Normalize(email);

        if (!IsValid(normalized))
        {
            throw new ArgumentException("Неправильний формат електронної пошти.", nameof(email));
        }

        return new Email(normalized);
    }

    /// <summary>
    /// Normalizes an email by trimming and converting to lowercase.
    /// </summary>
    /// <param name="email">The input email string.</param>
    /// <returns>A normalized email string.</returns>
    public static string Normalize(string email)
    {
        return email?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    /// <summary>
    /// Checks if an email string is valid.
    /// </summary>
    /// <param name="email">The email string to check.</param>
    /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
    public static bool IsValid(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
