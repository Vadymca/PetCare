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
    private static readonly Regex Regex = new(@"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,4}$", RegexOptions.Compiled);

    private Email(string value) => this.Value = value;

    /// <summary>
    /// Gets the email address string.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Email"/> instance after validating the email format.
    /// </summary>
    /// <param name="email">The email address to validate and encapsulate.</param>
    /// <returns>A new <see cref="Email"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the email is null, empty, or in an invalid format.</exception>
    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email))
        {
            throw new ArgumentException("Неправильний формат електронної пошти.", nameof(email));
        }

        return new Email(email);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
