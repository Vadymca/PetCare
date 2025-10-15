namespace PetCare.Domain.Specifications.Animal;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering animals by shelter ID.
/// </summary>
public sealed class AnimalsByShelterSpecification : Specification<Animal>
{
    private readonly Guid shelterId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalsByShelterSpecification"/> class.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter to filter animals by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="shelterId"/> is empty.</exception>
    public AnimalsByShelterSpecification(Guid shelterId)
    {
        if (shelterId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        }

        this.shelterId = shelterId;
    }

    /// <inheritdoc/>
    public override Expression<Func<Animal, bool>> ToExpression()
    {
        return a => a.ShelterId == this.shelterId;
    }
}
