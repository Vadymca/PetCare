using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        // Parameterless constructor for EF Core
        private Money() 
        { 
            Amount = 0;
            Currency = "UAH";
        }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "UAH")
        {
            if (amount < 0)
                throw new ArgumentException("Сума не може бути від'ємною.", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Валюта не може бути порожньою.", nameof(currency));

            return new Money(amount, currency.ToUpperInvariant());
        }

        public static Money Zero(string currency = "UAH") => new(0, currency);

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Неможливо додати гроші різних валют: {Currency} та {other.Currency}");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Неможливо відняти гроші різних валют: {Currency} та {other.Currency}");

            var result = Amount - other.Amount;
            if (result < 0)
                throw new InvalidOperationException("Результат не може бути від'ємним.");

            return new Money(result, Currency);
        }

        public Money Multiply(decimal factor)
        {
            if (factor < 0)
                throw new ArgumentException("Множник не може бути від'ємним.", nameof(factor));

            return new Money(Amount * factor, Currency);
        }

        public bool IsZero => Amount == 0;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString() => $"{Amount:F2} {Currency}";

        public static Money operator +(Money left, Money right) => left.Add(right);
        public static Money operator -(Money left, Money right) => left.Subtract(right);
        public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
        public static Money operator *(decimal factor, Money money) => money.Multiply(factor);

        public static bool operator >(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException($"Неможливо порівняти гроші різних валют: {left.Currency} та {right.Currency}");
            return left.Amount > right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException($"Неможливо порівняти гроші різних валют: {left.Currency} та {right.Currency}");
            return left.Amount < right.Amount;
        }

        public static bool operator >=(Money left, Money right) => left > right || left == right;
        public static bool operator <=(Money left, Money right) => left < right || left == right;
    }
}
