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
        var normalized = phone?.Trim() ?? string.Empty;

        if (!IsValid(normalized))
        {
            throw new ArgumentException(
                "Номер телефону повинен бути у дійсному форматі E.164 (наприклад, +380501112233).",
                nameof(phone));
        }

        return new PhoneNumber(normalized);
    }

    /// <summary>
    /// Checks if the given phone number string is valid E.164 format.
    /// </summary>
    /// <param name="phone">Phone number string to validate.</param>
    /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
    public static bool IsValid(string phone)
    {
        return !string.IsNullOrWhiteSpace(phone) && E164Regex.IsMatch(phone.Trim());
    }

    /// <summary>
    /// Formats the phone number in a readable format.
    /// For example, can insert spaces or dashes for easier reading.
    /// Currently returns the E.164 canonical format.
    /// </summary>
    /// <returns>The formatted phone number string.</returns>
    public string Format()
    {
        return this.Value;
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
