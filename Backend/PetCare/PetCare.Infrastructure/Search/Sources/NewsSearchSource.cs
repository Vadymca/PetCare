namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for news stored in memory.
/// </summary>
public sealed class NewsSearchSource : ISearchSource
{
    private const int MaxResults = 10;
    private const int SnippetLength = 120;

    private readonly INewsRepository newsRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewsSearchSource"/> class.
    /// </summary>
    /// <param name="newsRepository">News repository.</param>
    /// <exception cref="ArgumentNullException">Thrown when repository is null.</exception>
    public NewsSearchSource(INewsRepository newsRepository)
    {
        this.newsRepository = newsRepository
            ?? throw new ArgumentNullException(nameof(newsRepository));
    }

    /// <inheritdoc />
    public string Key => "news";

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

        var news = this.newsRepository.GetAll(language);

        var results = news
            .Where(n => this.Matches(n, normalizedQuery))
            .Select(n => new SearchResultItemDto(
                Title: n.Title,
                Slug: n.Id,
                Snippet: this.BuildSnippet(n, normalizedQuery)))
            .Take(MaxResults)
            .ToList();

        return Task.FromResult<IReadOnlyList<SearchResultItemDto>>(results);
    }

    private bool Matches(dynamic news, string query)
    {
        return this.Contains(news.Title, query)
            || this.Contains(news.DescriptionFirstPart, query)
            || this.Contains(news.DescriptionSecondPart, query);
    }

    private bool Contains(string? source, string query)
    {
        return !string.IsNullOrWhiteSpace(source)
            && source.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private string? BuildSnippet(dynamic news, string query)
    {
        var sources = new[]
        {
        news.DescriptionFirstPart,
        news.DescriptionSecondPart,
        news.Title,
        };

        foreach (var source in sources)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                continue;
            }

            var index = source.IndexOf(query, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                continue;
            }

            var start = Math.Max(0, index - (SnippetLength / 2));
            var length = Math.Min(SnippetLength, source.Length - start);

            var snippet = source.Substring(start, length).Trim();

            return start > 0
                ? $"…{snippet}"
                : snippet;
        }

        return null;
    }
}
