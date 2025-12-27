namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for static pages stored in memory.
/// </summary>
public sealed class StaticPageSearchSource : ISearchSource
{
    private const int MaxResults = 10;
    private const int SnippetLength = 140;

    private readonly IStaticPageRepository staticPageRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticPageSearchSource"/> class.
    /// </summary>
    /// <param name="staticPageRepository">Static page repository.</param>
    public StaticPageSearchSource(IStaticPageRepository staticPageRepository)
    {
        this.staticPageRepository = staticPageRepository
            ?? throw new ArgumentNullException(nameof(staticPageRepository));
    }

    /// <inheritdoc />
    public string Key => "pages";

    /// <inheritdoc />
    public Task<IReadOnlyList<SearchResultItemDto>> SearchAsync(
        string query,
        string language,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Task.FromResult<IReadOnlyList<SearchResultItemDto>>(
                Array.Empty<SearchResultItemDto>());
        }

        var normalizedQuery = query.Trim();

        var pages = this.staticPageRepository.GetAll();

        var results = pages
            .Select(page => this.TryMatch(page, normalizedQuery, language))
            .Where(result => result is not null)
            .Take(MaxResults)
            .Cast<SearchResultItemDto>()
            .ToList();

        return Task.FromResult<IReadOnlyList<SearchResultItemDto>>(results);
    }

    private SearchResultItemDto? TryMatch(
        dynamic page,
        string query,
        string language)
    {
        var title = page.Title[language];
        var text = page.Text[language];

        if (!this.Contains(title, query) && !this.Contains(text, query))
        {
            return null;
        }

        return new SearchResultItemDto(
            Title: title,
            Slug: page.Route,
            Snippet: this.BuildSnippet(text, query));
    }

    private bool Contains(string? source, string query)
    {
        return !string.IsNullOrWhiteSpace(source)
            && source.Contains(query, StringComparison.OrdinalIgnoreCase);
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
