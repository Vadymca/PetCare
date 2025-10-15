namespace PetCare.Domain.Specifications.Shelter;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

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
