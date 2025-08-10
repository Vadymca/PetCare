namespace PetCare.Domain.ValueObjects;
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a microchip identifier as a value object with validation.
/// </summary>
public sealed class MicrochipId : ValueObject
{
    private static readonly Regex MicrochipRegex = new(@"^[A-Z0-9]{5,20}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private MicrochipId(string value) => this.Value = value;

    /// <summary>
    /// Gets the microchip identifier value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new <see cref="MicrochipId"/> instance after validating the input format.
    /// </summary>
    /// <param name="value">The microchip identifier string.</param>
    /// <returns>A new <see cref="MicrochipId"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the microchip ID is null, empty, or invalid format.</exception>
    public static MicrochipId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Ідентифікатор мікрочіпа не може бути порожнім.", nameof(value));
        }

        value = value.Trim();

        if (!MicrochipRegex.IsMatch(value))
        {
            throw new ArgumentException("Неправильний формат ідентифікатора мікрочіпа.", nameof(value));
        }

        return new MicrochipId(value.ToUpperInvariant());
    }

    /// <summary>
    /// Validates whether the given microchip ID string matches the required format.
    /// </summary>
    /// <param name="value">The microchip ID string to validate.</param>
    /// <returns>True if valid; otherwise, false.</returns>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return MicrochipRegex.IsMatch(value.Trim());
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new[] { this.Value };
}
