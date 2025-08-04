// <copyright file="Article.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an article in the system.
/// </summary>
public sealed class Article : BaseEntity
{
    private readonly List<ArticleComment> comments = new();

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
    /// Gets the unique identifier of the article's category, if any. Can be null.
    /// </summary>
    public Guid? CategoryId { get; private set; }

    /// <summary>
    /// Gets navigation property to the category.
    /// </summary>
    public Category? Category { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the article's author, if any. Can be null.
    /// </summary>
    public Guid? AuthorId { get; private set; }

    /// <summary>
    /// Gets navigation property to the author.
    /// </summary>
    public User? Author { get; private set; }

    /// <summary>
    /// Gets the comments associated with this article.
    /// </summary>
    public IReadOnlyCollection<ArticleComment> Comments => this.comments.AsReadOnly();

    private ICollection<ArticleComment> CommentsPersistence
    {
        get => this.comments;
        set
        {
            this.comments.Clear();
            this.comments.AddRange(value);
        }
    }

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

    /// <summary>
    /// Publishes the article, setting its status to Published and updating the published date.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when article is already published.</exception>
    public void Publish()
    {
        if (this.Status == ArticleStatus.Published)
        {
            throw new InvalidOperationException("Стаття вже опублікована.");
        }

        this.Status = ArticleStatus.Published;
        this.PublishedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Archives the article, setting its status to Archived.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when article is already archived.</exception>
    public void Archive()
    {
        if (this.Status == ArticleStatus.Archived)
        {
            throw new InvalidOperationException("Стаття вже заархівована.");
        }

        this.Status = ArticleStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets or updates the thumbnail image URL.
    /// </summary>
    /// <param name="thumbnailUrl">The new thumbnail URL. Can be null to remove the thumbnail.</param>
    public void SetThumbnail(string? thumbnailUrl)
    {
        this.Thumbnail = thumbnailUrl;
        this.UpdatedAt = DateTime.UtcNow;
    }

    // === Коментарі ===

    /// <summary>
    /// Adds a new comment to the article.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who creates the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="parentCommentId">The identifier of the parent comment if this is a reply. Can be null.</param>
    /// <returns>The newly created <see cref="ArticleComment"/> instance.</returns>
    public ArticleComment AddComment(Guid userId, string content, Guid? parentCommentId = null)
    {
        var comment = ArticleComment.Create(this.Id, userId, content, parentCommentId);
        this.comments.Add(comment);
        this.UpdatedAt = DateTime.UtcNow;
        return comment;
    }

    /// <summary>
    /// Updates the content of a comment belonging to this article.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment to update.</param>
    /// <param name="newContent">The new content for the comment.</param>
    /// <exception cref="InvalidOperationException">Thrown when the comment with the specified ID is not found.</exception>
    public void UpdateComment(Guid commentId, string newContent)
    {
        var comment = this.GetCommentById(commentId);
        comment.UpdateContent(newContent);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Moderates a comment by changing its status.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment to moderate.</param>
    /// <param name="status">The new status to set for the comment.</param>
    /// <param name="moderatorId">The unique identifier of the moderator, if applicable.</param>
    /// <exception cref="InvalidOperationException">Thrown when the comment with the specified ID is not found.</exception>
    public void ModerateComment(Guid commentId, CommentStatus status, Guid? moderatorId = null)
    {
        var comment = this.GetCommentById(commentId);
        comment.SetStatus(status, moderatorId);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a like to a specific comment.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment to like.</param>
    /// <exception cref="InvalidOperationException">Thrown when the comment with the specified ID is not found.</exception>
    public void LikeComment(Guid commentId)
    {
        var comment = this.GetCommentById(commentId);
        comment.AddLike();
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a like from a specific comment.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment to unlike.</param>
    /// <exception cref="InvalidOperationException">Thrown when the comment with the specified ID is not found.</exception>
    public void UnlikeComment(Guid commentId)
    {
        var comment = this.GetCommentById(commentId);
        comment.RemoveLike();
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a comment from the article.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown when the comment with the specified ID is not found.</exception>
    public void RemoveComment(Guid commentId)
    {
        var comment = this.GetCommentById(commentId);
        this.comments.Remove(comment);
        this.UpdatedAt = DateTime.UtcNow;
    }

    private ArticleComment GetCommentById(Guid commentId)
    {
        var comment = this.comments.FirstOrDefault(c => c.Id == commentId);
        if (comment is null)
        {
            throw new InvalidOperationException("Коментар не знайдено в статті.");
        }

        return comment;
    }
}
