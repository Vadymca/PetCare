namespace PetCare.Infrastructure.Search.Sources;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;

/// <summary>
/// Search source for story articles stored in memory.
/// </summary>
public sealed class StoryArticleSearchSource : ISearchSource
{
    private const int MaxResults = 10;
    private const int SnippetLength = 120;

    private readonly IStoryArticleRepository storyArticleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoryArticleSearchSource"/> class.
    /// </summary>
    /// <param name="storyArticleRepository">Story article repository.</param>
    public StoryArticleSearchSource(IStoryArticleRepository storyArticleRepository)
    {
        this.storyArticleRepository = storyArticleRepository
            ?? throw new ArgumentNullException(nameof(storyArticleRepository));
    }

    /// <inheritdoc />
    public string Key => "stories";

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

        var stories = this.storyArticleRepository.GetAll(language);

        var results = stories
            .Where(s => this.Matches(s, normalizedQuery))
            .Select(s => new SearchResultItemDto(
                Title: s.Title,
                Slug: s.Slug,
                Snippet: this.BuildSnippet(s, normalizedQuery)))
            .Take(MaxResults)
            .ToList();

        return Task.FromResult<IReadOnlyList<SearchResultItemDto>>(results);
    }

    private bool Matches(dynamic story, string query)
    {
        return this.Contains(story.Title, query)
            || this.Contains(story.ShortContent, query);
    }

    private bool Contains(string? source, string query)
    {
        return !string.IsNullOrWhiteSpace(source)
            && source.Contains(query, StringComparison.OrdinalIgnoreCase);
    }

    private string? BuildSnippet(dynamic story, string query)
    {
        var source = story.ShortContent;

        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }

        var index = source.IndexOf(query, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return null;
        }

        var start = Math.Max(0, index - (SnippetLength / 2));
        var length = Math.Min(SnippetLength, source.Length - start);

        var snippet = source.Substring(start, length).Trim();

        return start > 0
            ? $"…{snippet}"
            : snippet;
    }
}
