namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a species of animal in the system.
/// </summary>
public sealed class Specie : AggregateRoot
{
    private readonly List<Breed> breeds = new();

    private Specie()
    {
        this.Name = Name.Create(string.Empty);
    }

    private Specie(Name name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the name of the species.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the read-only collection of breeds associated with the species.
    /// </summary>
    public IReadOnlyCollection<Breed> Breeds => this.breeds.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Specie"/> instance with the specified name.
    /// </summary>
    /// <param name="name">The name of the species.</param>
    /// <returns>A new instance of <see cref="Specie"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public static Specie Create(string name)
    {
        return new Specie(Name.Create(name));
    }

    /// <summary>
    /// Updates the name of the species.
    /// </summary>
    /// <param name="newName">The new name for the species.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newName"/> is invalid according to <see cref="Name.Create"/>.</exception>
    public void Rename(string newName)
    {
        this.Name = Name.Create(newName);
    }

    /// <summary>
    /// Adds a new breed to the species.
    /// </summary>
    /// <param name="breed">The breed to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="breed"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the breed already exists in the collection.</exception>
    public void AddBreed(Breed breed)
    {
        if (breed is null)
        {
            throw new ArgumentNullException(nameof(breed), "Порода не може бути null.");
        }

        if (this.breeds.Any(b => b.Id == breed.Id))
        {
            throw new InvalidOperationException("Ця порода вже додана до цього виду.");
        }

        this.breeds.Add(breed);
    }

    /// <summary>
    /// Removes a breed from the species.
    /// </summary>
    /// <param name="breedId">The ID of the breed to remove.</param>
    /// <returns><c>true</c> if the breed was removed; otherwise <c>false</c>.</returns>
    public bool RemoveBreed(Guid breedId)
    {
        var breed = this.breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
        {
            return false;
        }

        return this.breeds.Remove(breed);
    }
}
