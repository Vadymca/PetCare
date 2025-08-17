namespace PetCare.Domain.Specifications.AdoptionApplication;
using PetCare.Domain.Aggregates;
using System;
using System.Linq.Expressions;

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
