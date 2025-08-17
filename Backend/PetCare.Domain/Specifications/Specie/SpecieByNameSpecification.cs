namespace PetCare.Domain.Specifications.Specie;
using PetCare.Domain.Aggregates;
using System;
using System.Linq.Expressions;

/// <summary>
/// Specification for filtering species by name.
/// </summary>
public sealed class SpecieByNameSpecification : Specification<Specie>
{
    private readonly string name;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecieByNameSpecification"/> class.
    /// </summary>
    /// <param name="name">The name of the species to filter by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    public SpecieByNameSpecification(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ім'я не може бути нульовим або порожнім.", nameof(name));
        }

        this.name = name;
    }

    /// <inheritdoc />
    public override Expression<Func<Specie, bool>> ToExpression()
    {
        return s => s.Name.Value == this.name;
    }
}
