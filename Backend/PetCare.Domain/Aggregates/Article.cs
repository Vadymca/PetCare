// <copyright file="Article.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an article in the system.
/// </summary>
public sealed class Article : BaseEntity
{
    private Article()
    {
        this.Title = Title.Create(string.Empty);
        this.Content = string.Empty;
    }

    private Article(
        Title title,
        string content,
        Guid? categoryId,
        Guid? authorId,
        ArticleStatus status,
        string? thumbnail,
        DateTime publishedAt,
        DateTime updatedAt)
    {
        this.Title = title;
        this.Content = content;
        this.CategoryId = categoryId;
        this.AuthorId = authorId;
        this.Status = status;
        this.Thumbnail = thumbnail;
        this.PublishedAt = publishedAt;
        this.UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets the title of the article.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the content of the article.
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the article's category, if any. Can be null.
    /// </summary>
    public Guid? CategoryId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the article's author, if any. Can be null.
    /// </summary>
    public Guid? AuthorId { get; private set; }

    /// <summary>
    /// Gets the current status of the article.
    /// </summary>
    public ArticleStatus Status { get; private set; }

    /// <summary>
    /// Gets the URL of the article's thumbnail image, if any. Can be null.
    /// </summary>
    public string? Thumbnail { get; private set; }

    /// <summary>
    /// Gets the date and time when the article was published.
    /// </summary>
    public DateTime PublishedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the article was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Article"/> instance with the specified parameters.
    /// </summary>
    /// <param name="title">The title of the article.</param>
    /// <param name="content">The content of the article.</param>
    /// <param name="categoryId">The unique identifier of the article's category, if any. Can be null.</param>
    /// <param name="authorId">The unique identifier of the article's author, if any. Can be null.</param>
    /// <param name="status">The current status of the article.</param>
    /// <param name="thumbnail">The URL of the article's thumbnail image, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Article"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to <see cref="Title.Create"/>.</exception>
    public static Article Create(
        string title,
        string content,
        Guid? categoryId,
        Guid? authorId,
        ArticleStatus status,
        string? thumbnail = null)
    {
        var now = DateTime.UtcNow;
        return new Article(
            Title.Create(title),
            content,
            categoryId,
            authorId,
            status,
            thumbnail,
            publishedAt: now,
            updatedAt: now);
    }

    /// <summary>
    /// Updates the properties of the article, if provided.
    /// </summary>
    /// <param name="title">The new title of the article, if provided. If null, the title remains unchanged.</param>
    /// <param name="content">The new content of the article, if provided. If null, the content remains unchanged.</param>
    /// <param name="categoryId">The new category identifier of the article, if provided. If null, the category identifier remains unchanged.</param>
    /// <param name="status">The new status of the article, if provided. If null, the status remains unchanged.</param>
    /// <param name="thumbnail">The new URL of the article's thumbnail image, if provided. If null, the thumbnail remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to <see cref="Title.Create"/>.</exception>
    public void Update(
        string? title = null,
        string? content = null,
        Guid? categoryId = null,
        ArticleStatus? status = null,
        string? thumbnail = null)
    {
        if (title is not null)
        {
            this.Title = Title.Create(title);
        }

        if (content is not null)
        {
            this.Content = content;
        }

        if (categoryId is not null)
        {
            this.CategoryId = categoryId;
        }

        if (status is not null)
        {
            this.Status = status.Value;
        }

        if (thumbnail is not null)
        {
            this.Thumbnail = thumbnail;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }
}
