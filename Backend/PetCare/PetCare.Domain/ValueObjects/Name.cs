namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;

/// <summary>
/// Represents a name as a value object with validation.
/// </summary>
public sealed class Name : ValueObject
{
    private Name(string value) => this.Value = value;

    /// <summary>
    /// Gets the name string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="Name"/> instance after validating the input.
    /// </summary>
    /// <param name="value">The name string.</param>
    /// <returns>A new <see cref="Name"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the name is null or whitespace.</exception>
    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Ім'я не може бути порожнім.", nameof(value));
        }

        return new Name(value);
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
