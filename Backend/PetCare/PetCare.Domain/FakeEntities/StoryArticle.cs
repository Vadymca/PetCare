namespace PetCare.Domain.FakeEntities;

using System;

/// <summary>
/// Represents a story article for demonstration purposes.
/// </summary>
public sealed class StoryArticle
{
    /// <summary>
    /// Gets the title of the story article.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Gets the content of the story article.
    /// </summary>
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Gets the slug (URL-friendly identifier) of the story article.
    /// </summary>
    public string Slug { get; init; } = null!;

    /// <summary>
    /// Gets the short content or summary of the story article.
    /// </summary>
    public string ShortContent { get; init; } = null!;

    /// <summary>
    /// Gets the main image URL of the story article.
    /// </summary>
    public string Image { get; init; } = null!;
}
