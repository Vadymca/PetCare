namespace PetCare.Domain.Specifications.AdoptionApplication;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering adoption applications by animal ID.
/// </summary>
public sealed class AdoptionApplicationsByAnimalSpecification : Specification<AdoptionApplication>
{
    private readonly Guid animalId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationsByAnimalSpecification"/> class.
    /// </summary>
    /// <param name="animalId">The animal ID to filter applications by.</param>
    public AdoptionApplicationsByAnimalSpecification(Guid animalId)
    {
        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));
        }

        this.animalId = animalId;
    }

    /// <inheritdoc />
    public override Expression<Func<AdoptionApplication, bool>> ToExpression()
    {
        return a => a.AnimalId == this.animalId;
    }
}
