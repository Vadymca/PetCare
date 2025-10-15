namespace PetCare.Domain.Specifications;

using System;
using System.Linq.Expressions;

/// <summary>
/// Base class for specifications.
/// </summary>
/// <typeparam name="T">The type of entity this specification applies to.</typeparam>
public abstract class Specification<T>
{
    /// <summary>
    /// Returns the expression that represents the specification.
    /// </summary>
    /// <returns>An expression that can be used in LINQ queries.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Checks if a given entity satisfies the specification.
    /// </summary>
    /// <param name="entity">The entity to test.</param>
    /// <returns>True if the entity satisfies the specification; otherwise, false.</returns>
    public bool IsSatisfiedBy(T entity) => this.ToExpression().Compile()(entity);

    /// <summary>
    /// Combines the current specification with another using logical AND.
    /// </summary>
    /// <param name="other">The other specification to combine with.</param>
    /// <returns>A new specification representing the logical AND of the two specifications.</returns>
    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);

    /// <summary>
    /// Combines the current specification with another using logical OR.
    /// </summary>
    /// <param name="other">The other specification to combine with.</param>
    /// <returns>A new specification representing the logical OR of the two specifications.</returns>
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);

    /// <summary>
    /// Negates the current specification.
    /// </summary>
    /// <returns>A new specification representing the logical NOT of the current specification.</returns>
    public Specification<T> Not() => new NotSpecification<T>(this);
}

/// <summary>
/// Combines two specifications with a logical AND operation.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
internal sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> left;
    private readonly Specification<T> right;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class
    /// that combines two specifications using logical AND.
    /// </summary>
    /// <param name="left">The left-hand specification.</param>
    /// <param name="right">The right-hand specification.</param>
    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        this.left = left;
        this.right = right;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = this.left.ToExpression();
        var rightExpr = this.right.ToExpression();

        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, param),
            Expression.Invoke(rightExpr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

/// <summary>
/// Combines two specifications with a logical OR operation.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
internal sealed class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> left;
    private readonly Specification<T> right;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class
    /// that combines two specifications using logical OR.
    /// </summary>
    /// <param name="left">The left-hand specification.</param>
    /// <param name="right">The right-hand specification.</param>
    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        this.left = left;
        this.right = right;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = this.left.ToExpression();
        var rightExpr = this.right.ToExpression();

        var param = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(leftExpr, param),
            Expression.Invoke(rightExpr, param));

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

/// <summary>
/// Negates a specification with a logical NOT operation.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
internal sealed class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class
    /// that negates the provided specification.
    /// </summary>
    /// <param name="inner">The specification to negate.</param>
    public NotSpecification(Specification<T> inner)
    {
        this.inner = inner;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = this.inner.ToExpression();
        var param = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
