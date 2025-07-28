using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class ContactInfo : ValueObject
    {
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string? AlternativeContact { get; private set; }

        // Parameterless constructor for EF Core
        private ContactInfo() 
        { 
            Email = Email.Create("default@example.com");
            PhoneNumber = PhoneNumber.Create("+380000000000");
        }

        private ContactInfo(Email email, PhoneNumber phoneNumber, string? alternativeContact = null)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            AlternativeContact = alternativeContact;
        }

        public static ContactInfo Create(string email, string phoneNumber, string? alternativeContact = null)
        {
            return new ContactInfo(
                Email.Create(email),
                PhoneNumber.Create(phoneNumber),
                alternativeContact);
        }

        public static ContactInfo Create(Email email, PhoneNumber phoneNumber, string? alternativeContact = null)
        {
            return new ContactInfo(email, phoneNumber, alternativeContact);
        }

        public ContactInfo UpdateEmail(string newEmail)
        {
            return new ContactInfo(Email.Create(newEmail), PhoneNumber, AlternativeContact);
        }

        public ContactInfo UpdatePhoneNumber(string newPhoneNumber)
        {
            return new ContactInfo(Email, PhoneNumber.Create(newPhoneNumber), AlternativeContact);
        }

        public ContactInfo UpdateAlternativeContact(string? newAlternativeContact)
        {
            return new ContactInfo(Email, PhoneNumber, newAlternativeContact);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Email;
            yield return PhoneNumber;
            yield return AlternativeContact;
        }

        public override string ToString() => $"Email: {Email}, Phone: {PhoneNumber}" +
            (string.IsNullOrEmpty(AlternativeContact) ? "" : $", Alt: {AlternativeContact}");
    }
}
