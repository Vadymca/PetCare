using PetCare.Domain.Common;
using System.Text.RegularExpressions;

namespace PetCare.Domain.ValueObjects
{
    public sealed class PhoneNumber : ValueObject
    {
        private static readonly Regex E164Regex = new(@"^\+[1-9]\d{6,14}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private PhoneNumber() { Value = string.Empty; }

        private PhoneNumber(string value) => Value = value;

        public static PhoneNumber Create(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Номер телефону не може бути порожнім.", nameof(phone));

            phone = phone.Trim();

            if (!E164Regex.IsMatch(phone))
                throw new ArgumentException(
                    "Номер телефону повинен бути у дійсному форматі E.164 (наприклад, +380501112233).",
                    nameof(phone));

            return new PhoneNumber(phone);
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    }
}
