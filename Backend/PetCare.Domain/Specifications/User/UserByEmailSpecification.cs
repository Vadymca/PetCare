namespace PetCare.Domain.Specifications.User;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;
using System;
using System.Linq.Expressions;

/// <summary>
/// Specification for filtering users by email.
/// </summary>
public sealed class UserByEmailSpecification : Specification<User>
{
    private readonly string email;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserByEmailSpecification"/> class.
    /// </summary>
    /// <param name="email">The email to filter users by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> is null or empty.</exception>
    public UserByEmailSpecification(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email не може бути нульовим або порожнім.", nameof(email));
        }

        this.email = email;
    }

    /// <inheritdoc/>
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.Email == Email.Create(this.email);
    }
}
