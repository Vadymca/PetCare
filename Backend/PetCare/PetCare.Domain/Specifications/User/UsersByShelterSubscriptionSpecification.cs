namespace PetCare.Domain.Specifications.User;

using System;
using System.Linq;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering users by subscription to a specific shelter.
/// </summary>
public sealed class UsersByShelterSubscriptionSpecification : Specification<User>
{
    private readonly Guid shelterId;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersByShelterSubscriptionSpecification"/> class.
    /// </summary>
    /// <param name="shelterId">The ID of the shelter to filter users by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="shelterId"/> is empty.</exception>
    public UsersByShelterSubscriptionSpecification(Guid shelterId)
    {
        if (shelterId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        }

        this.shelterId = shelterId;
    }

    /// <summary>
    /// Returns the expression that represents the specification.
    /// </summary>
    /// <returns>An expression to filter users subscribed to the specified shelter.</returns>
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.ShelterSubscriptions.Any(s => s.ShelterId == this.shelterId);
    }
}
