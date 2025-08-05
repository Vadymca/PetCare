// <copyright file="ArticleTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Article"/> aggregate.
/// </summary>
public sealed class ArticleTests
{
    /// <summary>
    /// Tests creating an article with valid data initializes properties correctly.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldInitializeProperties()
    {
        var title = "Заголовок статті";
        var content = "Текст статті";
        Guid? categoryId = Guid.NewGuid();
        Guid? authorId = Guid.NewGuid();
        var status = ArticleStatus.Draft;
        string? thumbnail = "http://example.com/image.jpg";

        var article = Article.Create(title, content, categoryId, authorId, status, thumbnail);

        Assert.Equal(title, article.Title.Value);
        Assert.Equal(content, article.Content);
        Assert.Equal(categoryId, article.CategoryId);
        Assert.Equal(authorId, article.AuthorId);
        Assert.Equal(status, article.Status);
        Assert.Equal(thumbnail, article.Thumbnail);
        Assert.True((DateTime.UtcNow - article.PublishedAt).TotalSeconds < 5);
        Assert.True((DateTime.UtcNow - article.UpdatedAt).TotalSeconds < 5);
    }

    /// <summary>
    /// Tests updating an article updates the properties as expected.
    /// </summary>
    [Fact]
    public void Update_WithValidData_ShouldChangeProperties()
    {
        var article = Article.Create("Old title", "Old content", null, null, ArticleStatus.Draft);

        var newTitle = "Новий заголовок";
        var newContent = "Новий текст";
        Guid? newCategoryId = Guid.NewGuid();
        var newStatus = ArticleStatus.Published;
        var newThumbnail = "http://example.com/newimage.jpg";

        article.Update(newTitle, newContent, newCategoryId, newStatus, newThumbnail);

        Assert.Equal(newTitle, article.Title.Value);
        Assert.Equal(newContent, article.Content);
        Assert.Equal(newCategoryId, article.CategoryId);
        Assert.Equal(newStatus, article.Status);
        Assert.Equal(newThumbnail, article.Thumbnail);
    }

    /// <summary>
    /// Tests publishing an article sets status and updates dates.
    /// </summary>
    [Fact]
    public void Publish_WhenNotPublished_ShouldSetStatusPublishedAndUpdateDates()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var oldUpdatedAt = article.UpdatedAt;

        article.Publish();

        Assert.Equal(ArticleStatus.Published, article.Status);
        Assert.True(article.PublishedAt > DateTime.UtcNow.AddMinutes(-1));
        Assert.True(article.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests publishing an already published article throws an exception.
    /// </summary>
    [Fact]
    public void Publish_WhenAlreadyPublished_ShouldThrow()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Published);

        var ex = Assert.Throws<InvalidOperationException>(() => article.Publish());
        Assert.Contains("Стаття вже опублікована", ex.Message);
    }

    /// <summary>
    /// Tests archiving an article sets status and updates date.
    /// </summary>
    [Fact]
    public void Archive_WhenNotArchived_ShouldSetStatusArchivedAndUpdateDate()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Published);
        var oldUpdatedAt = article.UpdatedAt;

        article.Archive();

        Assert.Equal(ArticleStatus.Archived, article.Status);
        Assert.True(article.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests archiving an already archived article throws an exception.
    /// </summary>
    [Fact]
    public void Archive_WhenAlreadyArchived_ShouldThrow()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Archived);

        var ex = Assert.Throws<InvalidOperationException>(() => article.Archive());
        Assert.Contains("Стаття вже заархівована", ex.Message);
    }

    /// <summary>
    /// Tests setting the thumbnail updates the property and timestamp.
    /// </summary>
    [Fact]
    public void SetThumbnail_ShouldUpdateThumbnailAndUpdatedAt()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var oldUpdatedAt = article.UpdatedAt;

        article.SetThumbnail("http://example.com/newthumbnail.jpg");

        Assert.Equal("http://example.com/newthumbnail.jpg", article.Thumbnail);
        Assert.True(article.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests adding a comment adds it to the comments collection.
    /// </summary>
    [Fact]
    public void AddComment_ShouldAddCommentToCollection()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var userId = Guid.NewGuid();
        var content = "Коментар текст";

        var comment = article.AddComment(userId, content);

        Assert.Contains(comment, article.Comments);
        Assert.Equal(content, comment.Content);
        Assert.Equal(userId, comment.UserId);
        Assert.True(article.UpdatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    /// <summary>
    /// Tests updating a comment updates its content.
    /// </summary>
    [Fact]
    public void UpdateComment_WhenCommentExists_ShouldUpdateContent()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var comment = article.AddComment(Guid.NewGuid(), "Old content");

        article.UpdateComment(comment.Id, "New content");

        Assert.Equal("New content", comment.Content);
    }

    /// <summary>
    /// Tests updating a non-existing comment throws an exception.
    /// </summary>
    [Fact]
    public void UpdateComment_WhenCommentNotFound_ShouldThrow()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var unknownId = Guid.NewGuid();

        var ex = Assert.Throws<InvalidOperationException>(() => article.UpdateComment(unknownId, "Content"));
        Assert.Contains("Коментар не знайдено", ex.Message);
    }

    /// <summary>
    /// Tests moderating a comment updates its status and moderator ID.
    /// </summary>
    [Fact]
    public void ModerateComment_ShouldSetStatusAndModerator()
    {
        // Arrange
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var comment = article.AddComment(Guid.NewGuid(), "Some content");
        var moderatorId = Guid.NewGuid();

        // Act
        article.ModerateComment(comment.Id, CommentStatus.Approved, moderatorId);

        // Assert
        Assert.Equal(CommentStatus.Approved, comment.Status);
        Assert.Equal(moderatorId, comment.ModeratedBy);
        Assert.True(comment.UpdatedAt > comment.CreatedAt);
    }

    /// <summary>
    /// Tests liking a comment increments its like count.
    /// </summary>
    [Fact]
    public void LikeComment_ShouldIncrementLikes()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var comment = article.AddComment(Guid.NewGuid(), "Some content");
        var oldLikes = comment.Likes;

        article.LikeComment(comment.Id);

        Assert.Equal(oldLikes + 1, comment.Likes);
    }

    /// <summary>
    /// Tests unliking a comment decrements its like count.
    /// </summary>
    [Fact]
    public void UnlikeComment_ShouldDecrementLikes()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var comment = article.AddComment(Guid.NewGuid(), "Some content");
        article.LikeComment(comment.Id);
        var likesAfterLike = comment.Likes;

        article.UnlikeComment(comment.Id);

        Assert.Equal(likesAfterLike - 1, comment.Likes);
    }

    /// <summary>
    /// Tests removing a comment removes it from the comments collection.
    /// </summary>
    [Fact]
    public void RemoveComment_WhenExists_ShouldRemove()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var comment = article.AddComment(Guid.NewGuid(), "Content");

        article.RemoveComment(comment.Id);

        Assert.DoesNotContain(comment, article.Comments);
    }

    /// <summary>
    /// Tests removing a non-existing comment throws an exception.
    /// </summary>
    [Fact]
    public void RemoveComment_WhenNotFound_ShouldThrow()
    {
        var article = Article.Create("Title", "Content", null, null, ArticleStatus.Draft);
        var unknownId = Guid.NewGuid();

        var ex = Assert.Throws<InvalidOperationException>(() => article.RemoveComment(unknownId));
        Assert.Contains("Коментар не знайдено", ex.Message);
    }
}
