using PetCare.Domain.Common;
using System.Text.RegularExpressions;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex _regex = new(@"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,4}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private Email() { Value = string.Empty; }

        private Email(string value) => Value = value;

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email не може бути порожнім.", nameof(email));

            if (!_regex.IsMatch(email))
                throw new ArgumentException("Неправильний формат електронної пошти.", nameof(email));

            return new Email(email);
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
