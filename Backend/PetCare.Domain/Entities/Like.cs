namespace PetCare.Domain.Entities;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;

/// <summary>
/// Represents a like for any entity in the system.
/// </summary>
public sealed class Like : BaseEntity
{
    private Like()
    {
    }

    private Like(Guid userId, string likedEntity, Guid likedEntityId, Guid? articleCommentId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(likedEntity))
        {
            throw new ArgumentException("Тип сутності не може бути порожнім.", nameof(likedEntity));
        }

        if (likedEntityId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор сутності не може бути порожнім.", nameof(likedEntityId));
        }

        this.UserId = userId;
        this.LikedEntity = likedEntity;
        this.LikedEntityId = likedEntityId;
        this.ArticleCommentId = articleCommentId;
        this.CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the ID of the user who liked the entity.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who liked the entity.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the name of the entity type (e.g., "ArticleComment", "Article").
    /// </summary>
    public string LikedEntity { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the ID of the liked entity.
    /// </summary>
    public Guid LikedEntityId { get; private set; }

    /// <summary>
    /// Gets the creation date of the like.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets optional foreign key for ArticleComment.
    /// </summary>
    public Guid? ArticleCommentId { get; private set; }

    /// <summary>
    /// Gets optional navigation to the ArticleComment.
    /// </summary>
    public ArticleComment? ArticleComment { get; private set; }

    /// <summary>
    /// Creates a new like instance.
    /// </summary>
    /// <param name="userId">The ID of the user who liked the entity.</param>
    /// <param name="likedEntity">The type of the liked entity.</param>
    /// <param name="likedEntityId">The ID of the liked entity.</param>
    /// <param name="articleCommentId">The ID of the articleComment.</param>
    /// <returns>A new like instance.</returns>
    public static Like Create(Guid userId, string likedEntity, Guid likedEntityId, Guid? articleCommentId = null)
        => new(userId, likedEntity, likedEntityId, articleCommentId);

}
