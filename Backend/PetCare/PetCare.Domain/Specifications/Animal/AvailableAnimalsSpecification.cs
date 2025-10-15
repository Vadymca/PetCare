namespace PetCare.Domain.Specifications.Animal;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

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
