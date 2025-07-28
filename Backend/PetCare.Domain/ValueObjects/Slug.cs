using PetCare.Domain.Common;
using System.Text.RegularExpressions;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Slug : ValueObject
    {
        private static readonly Regex _slugRegex = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);

        public string Value { get; private set; }

        // Parameterless constructor for EF Core
        private Slug() { Value = string.Empty; }

        private Slug(string value) => Value = value;

        public static Slug Create(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));

            // Нормалізація: опускати, обрізати, замінювати пробіли, тире
            var normalized = slug.Trim().ToLowerInvariant();

            // Заміна пробілів та підкреслення на тире
            normalized = Regex.Replace(normalized, @"[\s_]+", "-");

            // Видалити неприпустимі символи
            normalized = Regex.Replace(normalized, @"[^a-z0-9\-]", "");

            // Заміна кілька тире на одне
            normalized = Regex.Replace(normalized, @"-+", "-");

            // Обрізати початкові або кінцеві тире
            normalized = normalized.Trim('-');

            if (string.IsNullOrWhiteSpace(normalized))
                throw new ArgumentException("Slug не містить допустимих символів після форматування.", nameof(slug));

            if (!_slugRegex.IsMatch(normalized))
                throw new ArgumentException("Slug не дійсний.", nameof(slug));

            if (normalized.Length > 64)
                throw new ArgumentException("Slug не може бути довшим за 64 символи.", nameof(slug));

            return new Slug(normalized);
        }

        protected override IEnumerable<object?> GetEqualityComponents() => new[] { Value };

        public override string ToString() => Value;

        public static implicit operator string(Slug slug) => slug.Value;
    }
}
