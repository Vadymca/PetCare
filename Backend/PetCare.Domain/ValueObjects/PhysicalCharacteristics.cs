using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class PhysicalCharacteristics : ValueObject
    {
        public float? Weight { get; private set; } // in kg
        public float? Height { get; private set; } // in cm
        public string? Color { get; private set; }

        // Parameterless constructor for EF Core
        private PhysicalCharacteristics() { }

        private PhysicalCharacteristics(float? weight, float? height, string? color)
        {
            Weight = weight;
            Height = height;
            Color = color;
        }

        public static PhysicalCharacteristics Create(float? weight = null, float? height = null, string? color = null)
        {
            if (weight.HasValue && weight.Value <= 0)
                throw new ArgumentException("Вага повинна бути більше нуля.", nameof(weight));

            if (height.HasValue && height.Value <= 0)
                throw new ArgumentException("Ріст повинен бути більше нуля.", nameof(height));

            if (!string.IsNullOrWhiteSpace(color) && color.Length > 50)
                throw new ArgumentException("Колір не може бути довшим за 50 символів.", nameof(color));

            return new PhysicalCharacteristics(weight, height, color?.Trim());
        }

        public PhysicalCharacteristics UpdateWeight(float? newWeight)
        {
            return Create(newWeight, Height, Color);
        }

        public PhysicalCharacteristics UpdateHeight(float? newHeight)
        {
            return Create(Weight, newHeight, Color);
        }

        public PhysicalCharacteristics UpdateColor(string? newColor)
        {
            return Create(Weight, Height, newColor);
        }

        public bool HasWeight => Weight.HasValue;
        public bool HasHeight => Height.HasValue;
        public bool HasColor => !string.IsNullOrWhiteSpace(Color);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Weight;
            yield return Height;
            yield return Color;
        }

        public override string ToString()
        {
            var parts = new List<string>();
            
            if (HasWeight) parts.Add($"Вага: {Weight:F1} кг");
            if (HasHeight) parts.Add($"Ріст: {Height:F1} см");
            if (HasColor) parts.Add($"Колір: {Color}");

            return parts.Count > 0 ? string.Join(", ", parts) : "Характеристики не вказані";
        }
    }
}
