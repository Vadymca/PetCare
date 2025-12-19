namespace PetCare.Domain.Abstractions.Repositories;

using System.Collections.Generic;
using PetCare.Domain.FakeEntities;

/// <summary>
/// Represents a repository interface for accessing story article entities.
/// </summary>
public interface IStoryArticleRepository
{
    /// <summary>
    /// Retrieves all story articles for a specified language.
    /// </summary>
    /// <param name="language">The language code for filtering articles.</param>
    /// <returns>A read-only list of story articles.</returns>
    IReadOnlyList<StoryArticle> GetAll(string language);

    /// <summary>
    /// Retrieves a story article by its slug and language.
    /// </summary>
    /// <param name="slug">The slug identifier of the story article.</param>
    /// <param name="language">The language code for the article.</param>
    /// <returns>The story article matching the slug and language, or null if not found.</returns>
    StoryArticle? GetBySlug(string slug, string language);
}
