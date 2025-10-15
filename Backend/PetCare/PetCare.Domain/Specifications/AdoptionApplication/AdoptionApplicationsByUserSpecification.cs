namespace PetCare.Domain.Specifications.AdoptionApplication;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering adoption applications by user ID.
/// </summary>
public sealed class AdoptionApplicationsByUserSpecification : Specification<AdoptionApplication>
{
    private readonly Guid userId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationsByUserSpecification"/> class.
    /// </summary>
    /// <param name="userId">The user ID to filter applications by.</param>
    public AdoptionApplicationsByUserSpecification(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        this.userId = userId;
    }

    /// <inheritdoc />
    public override Expression<Func<AdoptionApplication, bool>> ToExpression()
    {
        return a => a.UserId == this.userId;
    }
}
