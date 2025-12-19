namespace PetCare.Domain.Abstractions.Repositories;

using System.Collections.Generic;
using PetCare.Domain.FakeEntities;

/// <summary>
/// Represents a repository interface for accessing static page entities.
/// </summary>
public interface IStaticPageRepository
{
    /// <summary>
    /// Retrieves a static page by its route.
    /// </summary>
    /// <param name="route">The route of the static page.</param>
    /// <returns>The static page corresponding to the specified route, or null if not found.</returns>
    StaticPage? GetByRoute(string route);

    /// <summary>
    /// Retrieves all static pages.
    /// </summary>
    /// <returns>A read-only list of all static pages.</returns>
    IReadOnlyList<StaticPage> GetAll();
}
