namespace PetCare.Domain.Specifications.Shelter;
using PetCare.Domain.Aggregates;
using System;
using System.Linq.Expressions;

/// <summary>
/// Specification for filtering shelters that have free capacity.
/// </summary>
public sealed class SheltersWithFreeCapacitySpecification : Specification<Shelter>
{
    /// <inheritdoc />
    public override Expression<Func<Shelter, bool>> ToExpression()
    {
        return s => s.CurrentOccupancy < s.Capacity;
    }
}
