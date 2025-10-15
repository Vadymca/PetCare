namespace PetCare.Domain.Specifications.User;

using System.Linq.Expressions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Specification for filtering users by role.
/// </summary>
public sealed class UsersByRoleSpecification : Specification<User>
{
    private readonly UserRole role;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersByRoleSpecification"/> class.
    /// </summary>
    /// <param name="role">The role to filter users by.</param>
    public UsersByRoleSpecification(UserRole role)
    {
        this.role = role;
    }

    /// <summary>
    /// Returns the expression that represents the specification.
    /// </summary>
    /// <returns>An expression to filter users by the specified role.</returns>
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.Role == this.role;
    }
}
