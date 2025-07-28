using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class PersonName : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        // Parameterless constructor for EF Core
        private PersonName() 
        { 
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        private PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static PersonName Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(lastName));

            var trimmedFirstName = firstName.Trim();
            var trimmedLastName = lastName.Trim();

            if (trimmedFirstName.Length > 50)
                throw new ArgumentException("Ім'я не може бути довшим за 50 символів.", nameof(firstName));

            if (trimmedLastName.Length > 50)
                throw new ArgumentException("Прізвище не може бути довшим за 50 символів.", nameof(lastName));

            return new PersonName(trimmedFirstName, trimmedLastName);
        }

        public string FullName => $"{FirstName} {LastName}";

        public PersonName UpdateFirstName(string newFirstName)
        {
            return Create(newFirstName, LastName);
        }

        public PersonName UpdateLastName(string newLastName)
        {
            return Create(FirstName, newLastName);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }

        public override string ToString() => FullName;
    }
}
