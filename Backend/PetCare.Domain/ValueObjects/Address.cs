
using PetCare.Domain.Common;
using System.Text.RegularExpressions;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Address : ValueObject
    {
        private static readonly Regex AddressRegex =
                new(@"^(вул\.|пров\.|просп\.|пл\.|бульв\.)\s+\p{L}+.*?,\s*(№?\s*\d+[A-Za-zА-Яа-я]?),\s*м\.\s*\p{L}+$",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private Address() { Value = string.Empty; }

        private Address(string value) => Value = value;

        public static Address Create(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адреса не може бути порожньою.", nameof(address));

            var trimmed = address.Trim();

            if (trimmed.Length < 10 || trimmed.Length > 200)
                throw new ArgumentException("Адреса повинна містити від 10 до 200 символів.", nameof(address));

            if (!AddressRegex.IsMatch(trimmed))
                throw new ArgumentException("Формат адреси невірний. Приклад: 'вул. Головна, 12, м. Чернівці'", nameof(address));

            return new Address(trimmed);
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(Address address) => address.Value;
    }
}