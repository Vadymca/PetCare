namespace PetCare.Domain.Specifications.VolunteerTask;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering volunteer tasks by shelter ID.
/// </summary>
public sealed class VolunteerTasksByShelterSpecification : Specification<VolunteerTask>
{
    private readonly Guid shelterId;

    /// <summary>
    /// Initializes a new instance of the <see cref="VolunteerTasksByShelterSpecification"/> class.
    /// </summary>
    /// <param name="shelterId">The shelter ID to filter tasks by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="shelterId"/> is empty.</exception>
    public VolunteerTasksByShelterSpecification(Guid shelterId)
    {
        if (shelterId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        }

        this.shelterId = shelterId;
    }

    /// <inheritdoc/>
    public override Expression<Func<VolunteerTask, bool>> ToExpression()
    {
        return t => t.ShelterId == this.shelterId;
    }
}
