namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.SearchDtos;

/// <summary>
/// Provides unified search functionality across multiple sources.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Performs a global search across all available sources.
    /// </summary>
    /// <param name="query">Search query.</param>
    /// <param name="language">Language code (e.g. uk, en).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Grouped search results.</returns>
    Task<SearchResponseDto> SearchAsync(
        string query,
        string language,
        CancellationToken cancellationToken);
}
