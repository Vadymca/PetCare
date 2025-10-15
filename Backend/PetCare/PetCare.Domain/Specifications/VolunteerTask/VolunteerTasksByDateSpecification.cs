namespace PetCare.Domain.Specifications.VolunteerTask;

using System;
using System.Linq.Expressions;
using PetCare.Domain.Aggregates;

/// <summary>
/// Specification for filtering volunteer tasks by date.
/// </summary>
public sealed class VolunteerTasksByDateSpecification : Specification<VolunteerTask>
{
    private readonly DateOnly date;

    /// <summary>
    /// Initializes a new instance of the <see cref="VolunteerTasksByDateSpecification"/> class.
    /// </summary>
    /// <param name="date">The date to filter tasks by.</param>
    public VolunteerTasksByDateSpecification(DateOnly date)
    {
        this.date = date;
    }

    /// <inheritdoc/>
    public override Expression<Func<VolunteerTask, bool>> ToExpression()
    {
        return t => t.Date == this.date;
    }
}
