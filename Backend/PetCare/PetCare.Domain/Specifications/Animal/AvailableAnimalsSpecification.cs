namespace PetCare.Domain.Specifications.Animal;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using System.Linq.Expressions;

/// <summary>
/// Specification for filtering animals that are available for adoption.
/// </summary>
public sealed class AvailableAnimalsSpecification : Specification<Animal>
{
    /// <inheritdoc />
    public override Expression<Func<Animal, bool>> ToExpression()
    {
        return a => a.Status == AnimalStatus.Available;
    }
}
