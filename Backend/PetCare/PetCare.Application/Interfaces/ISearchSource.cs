namespace PetCare.Application.Interfaces;

using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;

/// <summary>
/// Defines a search source for a specific entity type.
/// </summary>
public interface ISearchSource
{
    /// <summary>
    /// Gets the unique key of the search source (e.g. animals, shelters, news).
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Performs a search within the source.
    /// </summary>
    /// <param name="query">Search query.</param>
    /// <param name="language">Language code (e.g. uk, en).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of search results.</returns>
    Task<IReadOnlyList<SearchResultItemDto>> SearchAsync(
        string query,
        string language,
        CancellationToken cancellationToken);
}
