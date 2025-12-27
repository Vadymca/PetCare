namespace PetCare.Infrastructure.Search;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Provides unified search functionality across multiple search sources.
/// </summary>
public sealed class SearchService : ISearchService
{
    private readonly IReadOnlyDictionary<string, ISearchSource> sources;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchService"/> class.
    /// </summary>
    /// <param name="sources">Available search sources.</param>
    /// <exception cref="ArgumentNullException">Thrown when sources are null.</exception>
    public SearchService(IEnumerable<ISearchSource> sources)
    {
        ArgumentNullException.ThrowIfNull(sources);

        this.sources = sources.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public async Task<SearchResponseDto> SearchAsync(
        string query,
        string language,
        CancellationToken cancellationToken)
    {
        var resultsList = new List<IReadOnlyList<SearchResultItemDto>>();

        foreach (var source in this.sources.Values)
        {
            var res = await source.SearchAsync(query, language, cancellationToken);
            resultsList.Add(res);
        }

        return new SearchResponseDto(
            Animals: this.GetResults("animals", resultsList),
            Shelters: this.GetResults("shelters", resultsList),
            Projects: this.GetResults("projects", resultsList),
            News: this.GetResults("news", resultsList),
            Stories: this.GetResults("stories", resultsList),
            Pages: this.GetResults("pages", resultsList));
    }

    private IReadOnlyList<SearchResultItemDto> GetResults(
        string key,
        IReadOnlyList<IReadOnlyList<SearchResultItemDto>> allResults)
    {
        if (!this.sources.TryGetValue(key, out var source))
        {
            return Array.Empty<SearchResultItemDto>();
        }

        var index = this.sources.Values.ToList().IndexOf(source);
        return index >= 0 ? allResults[index] : Array.Empty<SearchResultItemDto>();
    }
}
