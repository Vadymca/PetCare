using PetCare.Domain.Common;
using System.Text.RegularExpressions;

namespace PetCare.Domain.ValueObjects
{
    public sealed class MicrochipId : ValueObject
    {
        private static readonly Regex MicrochipRegex = new(@"^[A-Z0-9]{5,20}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private MicrochipId() { Value = string.Empty; }

        private MicrochipId(string value) => Value = value;

        public static MicrochipId Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ідентифікатор мікрочіпа не може бути порожнім.", nameof(value));

            value = value.Trim();

            if (!MicrochipRegex.IsMatch(value))
                throw new ArgumentException("Неправильний формат ідентифікатора мікрочіпа.", nameof(value));

            return new MicrochipId(value.ToUpperInvariant());
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(MicrochipId microchipId) => microchipId.Value;
    }
}
