namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;

/// <summary>
/// Represents a postal address as a value object with validation.
/// </summary>
public sealed class Address : ValueObject
{
    private Address(string value) => this.Value = value;

    /// <summary>
    /// Gets the address string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Address"/> instance without strict format validation.
    /// </summary>
    /// <param name="address">The address string.</param>
    /// <returns>A new <see cref="Address"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the address is null or empty.</exception>
    public static Address Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Адреса не може бути порожньою.", nameof(address));
        }

        var trimmed = address.Trim();

        if (trimmed.Length < 3 || trimmed.Length > 200)
        {
            throw new ArgumentException("Адреса повинна містити від 3 до 200 символів.", nameof(address));
        }

        return new Address(trimmed);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
