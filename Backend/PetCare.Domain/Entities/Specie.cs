using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

namespace PetCare.Domain.Entities
{
    public sealed class Specie : BaseEntity
    {
        public Name Name { get; private set; }

        // EF Core навігація
        public ICollection<Breed> Breeds { get; private set; } = new List<Breed>();

        private Specie() { }
    
        private Specie(Name name)
        {
            Name = name;
        }
        public static Specie Create(string name) =>
            new(Name.Create(name));
        public void Rename(string newName)
        {
            Name = Name.Create(newName);
        }
    }
}
