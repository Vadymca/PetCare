﻿using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Title : ValueObject
    {
        private const int MaxLength = 255;
        public string Value { get; }

        private Title(string value) => Value = value;

        public static Title Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Заголовок не може бути порожнім.", nameof(title));

            if (title.Length > MaxLength)
                throw new ArgumentException($"Назва не може бути довшою за {MaxLength} символів.", nameof(title));

            return new Title(title);
        }

        protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };
        public override string ToString() => Value;
    }
}
