namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for animal aid requests (projects).
/// </summary>
public sealed class ProjectSearchSource : ISearchSource
{
    private const int MaxResults = 10;
    private const int SnippetLength = 140;

    private readonly IShelterRepository shelterRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectSearchSource"/> class using the specified shelter repository.
    /// </summary>
    /// <param name="shelterRepository">The repository used to access shelter data. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if shelterRepository is null.</exception>
    public ProjectSearchSource(IShelterRepository shelterRepository)
    {
        this.shelterRepository = shelterRepository
            ?? throw new ArgumentNullException(nameof(shelterRepository));
    }

    /// <inheritdoc />
    public string Key => "projects";

    /// <inheritdoc />
    public async Task<IReadOnlyList<SearchResultItemDto>> SearchAsync(
        string query,
        string language,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<SearchResultItemDto>();
        }

        var items = await this.shelterRepository.SearchProjectAsync(
            query.Trim(),
            MaxResults,
            cancellationToken);

        var results = new List<SearchResultItemDto>(items.Count);

        foreach (var item in items)
        {
            results.Add(new SearchResultItemDto(
                Title: item.Name,
                Slug: item.Slug,
                Snippet: this.BuildSnippet(item.Snippet, query)));
        }

        return results;
    }

    private string? BuildSnippet(string? text, string query)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var index = text.IndexOf(query, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return null;
        }

        var start = Math.Max(0, index - (SnippetLength / 2));
        var length = Math.Min(SnippetLength, text.Length - start);

        var snippet = text.Substring(start, length).Trim();

        return start > 0
            ? $"…{snippet}"
            : snippet;
    }
}
