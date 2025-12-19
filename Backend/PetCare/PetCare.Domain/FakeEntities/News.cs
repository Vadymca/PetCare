namespace PetCare.Domain.FakeEntities;

using System;
using System.Collections.Generic;

/// <summary>
/// The News entity represents a news article or update within the system.
/// </summary>
public sealed class News(
    string id,
    DateTime date,
    string titleShort,
    string title,
    string descriptionFirstPart,
    string subTitle,
    string descriptionSecondPart,
    List<string> photos,
    string conclusion)
{
    /// <summary>
    /// Gets the unique identifier of the news item.
    /// </summary>
    public string Id { get; init; } = id;

    /// <summary>
    /// Gets the date of the news item.
    /// </summary>
    public DateTime Date { get; init; } = date;

    /// <summary>
    /// Gets the short title of the news item.
    /// </summary>
    public string TitleShort { get; init; } = titleShort;

    /// <summary>
    /// Gets the full title of the news item.
    /// </summary>
    public string Title { get; init; } = title;

    /// <summary>
    /// Gets the first part of the news description.
    /// </summary>
    public string DescriptionFirstPart { get; init; } = descriptionFirstPart;

    /// <summary>
    /// Gets the subtitle of the news item.
    /// </summary>
    public string SubTitle { get; init; } = subTitle;

    /// <summary>
    /// Gets the second part of the news description.
    /// </summary>
    public string DescriptionSecondPart { get; init; } = descriptionSecondPart;

    /// <summary>
    /// Gets the list of photo URLs associated with the news item.
    /// </summary>
    public IReadOnlyList<string> Photos { get; init; } = photos.AsReadOnly();

    /// <summary>
    /// Gets the conclusion of the news item.
    /// </summary>
    public string Conclusion { get; init; } = conclusion;
}
