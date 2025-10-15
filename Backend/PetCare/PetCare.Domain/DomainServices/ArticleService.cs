namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing articles and article comments.
/// </summary>
public sealed class ArticleService : IArticleService
{
    /// <inheritdoc/>
    public async Task AddArticleAsync(User user, Article article, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (article is null)
        {
            throw new ArgumentNullException(nameof(article));
        }

        user.AddArticle(article, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveArticleAsync(User user, Guid articleId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.RemoveArticle(articleId, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task AddCommentAsync(User user, Guid articleId, ArticleComment comment, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (comment is null)
        {
            throw new ArgumentNullException(nameof(comment));
        }

        user.AddArticleComment(comment, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveCommentAsync(User user, Guid articleId, Guid commentId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var removed = user.RemoveArticleComment(commentId, requestingUserId);
        await Task.FromResult(removed);
    }
}
