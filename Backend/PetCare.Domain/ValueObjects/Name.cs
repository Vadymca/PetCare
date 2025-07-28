using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Name : ValueObject
    {
        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private Name() { Value = string.Empty; }

        private Name(string value) => Value = value;

        public static Name Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(value));

            var trimmed = value.Trim();
            if (trimmed.Length > 100)
                throw new ArgumentException("Ім'я не може бути довшим за 100 символів.", nameof(value));

            return new Name(trimmed);
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(Name name) => name.Value;
    }
}
