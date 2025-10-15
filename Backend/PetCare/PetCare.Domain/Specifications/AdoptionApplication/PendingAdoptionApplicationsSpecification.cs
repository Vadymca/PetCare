namespace PetCare.Domain.Specifications.AdoptionApplication;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering adoption applications with pending status.
/// </summary>
public sealed class PendingAdoptionApplicationsSpecification : Specification<AdoptionApplication>
{
    /// <inheritdoc />
    public override Expression<Func<AdoptionApplication, bool>> ToExpression()
    {
        return a => a.Status == Domain.Enums.AdoptionStatus.Pending;
    }
}
