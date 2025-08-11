namespace PetCare.Domain.Entities;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a comment associated with an article, including its content, status, and metadata.
/// </summary>
public sealed class ArticleComment : BaseEntity
{
    private readonly List<Like> likes = new();

    private ArticleComment()
    {
        this.Content = string.Empty;
    }

    private ArticleComment(
        Guid articleId,
        Guid userId,
        Guid? parentCommentId,
        string content,
        CommentStatus status,
        Guid? moderatedBy,
        DateTime createdAt,
        DateTime updatedAt)
    {
        if (articleId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор статті не може бути порожнім.", nameof(articleId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Вміст не може бути порожнім.", nameof(content));
        }

        this.ArticleId = articleId;
        this.UserId = userId;
        this.ParentCommentId = parentCommentId;
        this.Content = content;
        this.Status = status;
        this.ModeratedBy = moderatedBy;
        this.CreatedAt = createdAt;
        this.UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets the unique identifier of the article the comment is associated with.
    /// </summary>
    public Guid ArticleId { get; private set; }

    /// <summary>
    /// Gets the article that this comment belongs to.
    /// </summary>
    public Article? Article { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user who created the comment.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who created this comment.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the parent comment, if this is a reply. Can be null.
    /// </summary>
    public Guid? ParentCommentId { get; private set; }

    /// <summary>
    /// Gets the parent comment if this is a reply. Can be null.
    /// </summary>
    public ArticleComment? ParentComment { get; private set; }

    /// <summary>
    /// Gets the content of the comment.
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Gets the collection of likes for this comment.
    /// </summary>
    public IReadOnlyCollection<Like> Likes => this.likes.AsReadOnly();

    /// <summary>
    /// Gets the status of the comment (e.g., Pending, Approved, Rejected).
    /// </summary>
    public CommentStatus Status { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user who moderated the comment, if applicable. Can be null.
    /// </summary>
    public Guid? ModeratedBy { get; private set; }

    /// <summary>
    /// Gets the date and time when the comment was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the comment was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="ArticleComment"/> instance with the specified parameters.
    /// </summary>
    /// <param name="articleId">The unique identifier of the article the comment is associated with.</param>
    /// <param name="userId">The unique identifier of the user who created the comment.</param>
    /// <param name="content">The content of the comment.</param>
    /// <param name="parentCommentId">The unique identifier of the parent comment, if this is a reply. Can be null.</param>
    /// <returns>A new instance of <see cref="ArticleComment"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="articleId"/> or <paramref name="userId"/> is empty, or when <paramref name="content"/> is null or empty.</exception>
    public static ArticleComment Create(
        Guid articleId,
        Guid userId,
        string content,
        Guid? parentCommentId = null)
    {
        return new ArticleComment(
            articleId,
            userId,
            parentCommentId,
            content,
            CommentStatus.Pending,
            null,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Updates the content of the comment and sets the updated timestamp.
    /// </summary>
    /// <param name="content">The new content of the comment.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="content"/> is null or empty.</exception>
    public void UpdateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Вміст не може бути порожнім.", nameof(content));
        }

        this.Content = content;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the status of the comment and updates the moderated user and timestamp.
    /// </summary>
    /// <param name="status">The new status of the comment (e.g., Pending, Approved, Rejected).</param>
    /// <param name="moderatedBy">The unique identifier of the user who moderated the comment, if applicable. Can be null.</param>
    public void SetStatus(CommentStatus status, Guid? moderatedBy)
    {
        this.Status = status;
        this.ModeratedBy = moderatedBy;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a like from the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user liking the comment.</param>
    public void AddLike(Guid userId)
    {
        if (this.likes.Any(l => l.UserId == userId))
        {
            return;
        }

        this.likes.Add(Like.Create(userId, nameof(ArticleComment), this.Id));
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a like from the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user unliking the comment.</param>
    public void RemoveLike(Guid userId)
    {
        var like = this.likes.FirstOrDefault(l => l.UserId == userId);
        if (like != null)
        {
            this.likes.Remove(like);
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
