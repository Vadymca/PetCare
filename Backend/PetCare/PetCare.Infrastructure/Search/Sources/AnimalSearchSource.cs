namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for animals stored in the database.
/// </summary>
public sealed class AnimalSearchSource : ISearchSource
{
    private const int MaxResults = 5;
    private const int SnippetLength = 120;

    private readonly IAnimalRepository animalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalSearchSource"/> class.
    /// </summary>
    /// <param name="animalRepository">The animal repository.</param>
    public AnimalSearchSource(IAnimalRepository animalRepository)
    {
        this.animalRepository = animalRepository
            ?? throw new ArgumentNullException(nameof(animalRepository));
    }

    /// <inheritdoc />
    public string Key => "animals";

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

        var items = await this.animalRepository.SearchAnimalsAsync(
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
