namespace PetCare.Domain.ValueObjects;

using System.Text.Json.Serialization;
using PetCare.Domain.Common;

/// <summary>
/// Represents a birthday as a value object, including time in UTC.
/// </summary>
public sealed class Birthday : ValueObject
{
    private static readonly DateTime MinDate = DateTime.UtcNow.AddYears(-120);
    private static readonly DateTime MaxDate = DateTime.UtcNow;

    [JsonConstructor]
    private Birthday(DateTime value) => this.Value = value;

    /// <summary>
    /// Gets the value of the birthday in UTC.
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// Creates a new <see cref="Birthday"/> instance after validating the input.
    /// </summary>
    /// <param name="date">The birthday date (UTC).</param>
    /// <returns>A new instance of <see cref="Birthday"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date is in the future or unreasonably far in the past.</exception>
    public static Birthday Create(DateTime date)
    {
        var utcDate = date.ToUniversalTime();

        if (utcDate > MaxDate)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Дата народження не може бути в майбутньому.");
        }

        if (utcDate < MinDate)
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Дата народження надто стара для системи.");
        }

        return new Birthday(utcDate);
    }

    /// <summary>
    /// Checks if the provided date is valid as a birthday.
    /// </summary>
    /// <param name="date">The birthday date to validate.</param>
    /// <returns>True if the date is within valid bounds; otherwise, false.</returns>
    public static bool IsValid(DateTime date)
    {
        var utcDate = date.ToUniversalTime();
        return utcDate <= MaxDate && utcDate >= MinDate;
    }

    /// <summary>
    /// Calculates the age in years based on the current date.
    /// </summary>
    /// <returns>The age in complete years.</returns>
    public int CalculateAge()
    {
        var today = DateTime.UtcNow;
        int age = today.Year - this.Value.Year;

        if (today < this.Value.AddYears(age))
        {
            age--;
        }

        return age;
    }

    /// <inheritdoc/>
    public override string ToString() => this.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents() => new object[] { this.Value };
}
