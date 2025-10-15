namespace PetCare.Domain.Specifications.Shelter;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering shelters by manager ID.
/// </summary>
public sealed class SheltersByManagerSpecification : Specification<Shelter>
{
    private readonly Guid managerId;

    /// <summary>
    /// Initializes a new instance of the <see cref="SheltersByManagerSpecification"/> class.
    /// </summary>
    /// <param name="managerId">The manager ID to filter shelters by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="managerId"/> is empty.</exception>
    public SheltersByManagerSpecification(Guid managerId)
    {
        if (managerId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор менеджера не може бути порожнім.", nameof(managerId));
        }

        this.managerId = managerId;
    }

    /// <inheritdoc />
    public override Expression<Func<Shelter, bool>> ToExpression()
    {
        return s => s.ManagerId == this.managerId;
    }
}
