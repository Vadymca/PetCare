using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

namespace PetCare.Domain.Entities
{
    public sealed class Breed : BaseEntity
    {
        public Guid SpeciesId { get; private set; }
        public Name Name { get; private set; }
        public string? Description { get; private set; }

        // EF Core навігація
        public Specie? Specie { get; private set; }

        private Breed() { }

        private Breed(Guid speciesId, Name name, string? description)
        {
            SpeciesId = speciesId;
            Name = name;
            Description = description;
        }

        public static Breed Create(Guid speciesId, string name, string? description)
        {
            if (speciesId == Guid.Empty)
                throw new ArgumentException("Обов'язкова ідентифікація виду.", nameof(speciesId));
            return new Breed(
                speciesId,
                Name.Create(name),
                description);
        }

        public void Update(
            string? name = null,
            string? description = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = Name.Create(name);
            if (description is not null) Description = description;
        }
    }
}
