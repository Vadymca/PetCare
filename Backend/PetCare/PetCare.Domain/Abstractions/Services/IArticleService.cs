namespace PetCare.Domain.Abstractions.Services;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Defines operations for managing articles and article comments.
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Adds a new article for a user.
    /// </summary>
    /// <param name="user">The user creating the article.</param>
    /// <param name="article">The article to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddArticleAsync(User user, Article article, Guid requestingUserId);

    /// <summary>
    /// Removes an article for a user.
    /// </summary>
    /// <param name="user">The user who owns the article.</param>
    /// <param name="articleId">The unique identifier of the article to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveArticleAsync(User user, Guid articleId, Guid requestingUserId);

    /// <summary>
    /// Adds a new comment to an article.
    /// </summary>
    /// <param name="user">The user adding the comment.</param>
    /// <param name="articleId">The ID of the article.</param>
    /// <param name="comment">The comment to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddCommentAsync(User user, Guid articleId, ArticleComment comment, Guid requestingUserId);

    /// <summary>
    /// Removes a comment from an article.
    /// </summary>
    /// <param name="user">The user who owns the comment.</param>
    /// <param name="articleId">The ID of the article.</param>
    /// <param name="commentId">The ID of the comment to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveCommentAsync(User user, Guid articleId, Guid commentId, Guid requestingUserId);
}
