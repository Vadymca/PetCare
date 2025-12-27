namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for animals stored in the database.
/// Supports search by Name, Description, Breed and Species in two languages.
/// </summary>
public sealed class AnimalSearchSource : ISearchSource
{
    private const int MaxResults = 10;
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
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
        {
            return Array.Empty<SearchResultItemDto>();
        }

        var trimmedQuery = query.Trim();

        var items = await this.animalRepository.SearchAnimalsAsync(
            trimmedQuery,
            MaxResults,
            cancellationToken);

        var results = new List<SearchResultItemDto>(items.Count);

        foreach (var item in items)
        {
            var snippet = this.BuildSnippet(item.Snippet, trimmedQuery);
            results.Add(new SearchResultItemDto(
                Title: item.Name,
                Slug: item.Slug,
                Snippet: snippet));
        }

        return results;
    }

    /// <summary>
    /// Builds a snippet for search result.
    /// If query found in text, returns fragment around it.
    /// Otherwise returns first sentence.
    /// </summary>
    private string? BuildSnippet(string? text, string query)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        // Try to find the query in text (case-insensitive)
        var index = text.IndexOf(query, StringComparison.OrdinalIgnoreCase);
        if (index >= 0)
        {
            var start = Math.Max(0, index - (SnippetLength / 2));
            var length = Math.Min(SnippetLength, text.Length - start);
            var snippet = text.Substring(start, length).Trim();

            return start > 0 ? $"…{snippet}" : snippet;
        }

        // Fallback: return first sentence
        var match = Regex.Match(text, @"^.*?[.!?](\s|$)");
        if (match.Success)
        {
            return match.Value.Trim();
        }

        // If no sentence ending, return start of text
        return text.Length <= SnippetLength
            ? text
            : text.Substring(0, SnippetLength).Trim() + "…";
    }
}
