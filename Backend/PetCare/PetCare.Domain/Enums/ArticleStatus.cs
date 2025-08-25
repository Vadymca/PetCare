namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the publication status of an article.
/// </summary>
public enum ArticleStatus
{
    /// <summary>
    /// The article is a draft and not yet published.
    /// </summary>
    Draft,

    /// <summary>
    /// The article has been published and is publicly available.
    /// </summary>
    Published,

    /// <summary>
    /// The article is archived and no longer actively displayed.
    /// </summary>
    Archived,
}
