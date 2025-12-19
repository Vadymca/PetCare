namespace PetCare.Domain.Abstractions.Repositories;

using System.Collections.Generic;
using PetCare.Domain.FakeEntities;

/// <summary>
/// Represents a repository interface for accessing news entities.
/// </summary>
public interface INewsRepository
{
    /// <summary>
    /// Retrieves all news items in the specified language.
    /// </summary>
    /// <param name="language">The language code for the news items (default is "uk").</param>
    /// <returns>A read-only list of news items.</returns>
    IReadOnlyList<News> GetAll(string language = "uk");

    /// <summary>
    /// Retrieves a news item by its unique identifier and language.
    /// </summary>
    /// <param name="id">The unique identifier of the news item.</param>
    /// <param name="language">The language code for the news item (default is "uk").</param>
    /// <returns>The news item with the specified identifier.</returns>
    News? GetById(string id, string language = "uk");
}
