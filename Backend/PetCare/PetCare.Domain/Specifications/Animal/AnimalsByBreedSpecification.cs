namespace PetCare.Domain.Specifications.Animal;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering animals by breed ID.
/// </summary>
public sealed class AnimalsByBreedSpecification : Specification<Animal>
{
    private readonly Guid breedId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalsByBreedSpecification"/> class.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to filter animals by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="breedId"/> is empty.</exception>
    public AnimalsByBreedSpecification(Guid breedId)
    {
        if (breedId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор породи не може бути порожнім.", nameof(breedId));
        }

        this.breedId = breedId;
    }

    /// <inheritdoc />
    public override Expression<Func<Animal, bool>> ToExpression()
    {
        return a => a.BreedId == this.breedId;
    }
}
