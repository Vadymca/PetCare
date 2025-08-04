// <copyright file="SuccessStory.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a success story related to an animal in the system.
/// </summary>
public sealed class SuccessStory : BaseEntity
{
    private readonly List<string> photos = new();
    private readonly List<string> videos = new();

    private SuccessStory()
    {
        this.Title = Title.Create(string.Empty);
        this.Content = string.Empty;
    }

    private SuccessStory(
        Guid animalId,
        Guid? userId,
        Title title,
        string content,
        List<string>? photos,
        List<string>? videos)
    {
        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Контент обов'язковий.", nameof(content));
        }

        this.AnimalId = animalId;
        this.UserId = userId;
        this.Title = title;
        this.Content = content;
        this.photos = photos ?? new List<string>();
        this.videos = videos ?? new List<string>();
        this.Views = 0;
        this.PublishedAt = DateTime.UtcNow;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the title of the success story.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the content of the success story.
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the success story.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the list of video URLs for the success story.
    /// </summary>
    public IReadOnlyList<string> Videos => this.videos.AsReadOnly();

    /// <summary>
    /// Gets the date and time when the success story was published.
    /// </summary>
    public DateTime PublishedAt { get; private set; }

    /// <summary>
    /// Gets the number of views for the success story.
    /// </summary>
    public int Views { get; private set; }

    /// <summary>
    /// Gets the date and time when the success story was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the success story was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal associated with the success story.
    /// </summary>
    public Guid AnimalId { get; private set; }

    /// <summary>
    /// Gets the animal associated with the success story.
    /// </summary>
    public Animal? Animal { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user who created the success story, if any. Can be null.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the user who created the success story, if any.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Creates a new <see cref="SuccessStory"/> instance with the specified parameters.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal associated with the success story.</param>
    /// <param name="userId">The unique identifier of the user who created the success story, if any. Can be null.</param>
    /// <param name="title">The title of the success story.</param>
    /// <param name="content">The content of the success story.</param>
    /// <param name="photos">The list of photo URLs for the success story, if any. Can be null.</param>
    /// <param name="videos">The list of video URLs for the success story, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="SuccessStory"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="animalId"/> is an empty GUID or <paramref name="content"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to the <see cref="Title.Create"/> method.</exception>
    public static SuccessStory Create(
        Guid animalId,
        Guid? userId,
        string title,
        string content,
        List<string>? photos = null,
        List<string>? videos = null)
    {
        return new SuccessStory(
            animalId,
            userId,
            Title.Create(title),
            content,
            photos,
            videos);
    }

    /// <summary>
    /// Increments the view count of the success story by one.
    /// </summary>
    public void IncrementViews()
    {
        this.Views++;
    }

    /// <summary>
    /// Updates the success story's properties with the provided values.
    /// </summary>
    /// <param name="title">The new title of the success story, if provided. If null, the title remains unchanged.</param>
    /// <param name="content">The new content of the success story, if provided. If null, the content remains unchanged.</param>
    /// <param name="photos">The new list of photo URLs for the success story, if provided. If null, the photos remain unchanged.</param>
    /// <param name="videos">The new list of video URLs for the success story, if provided. If null, the videos remain unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to the <see cref="Title.Create"/> method.</exception>
    public void Update(
        string? title = null,
        string? content = null,
        List<string>? photos = null,
        List<string>? videos = null)
    {
        if (title is not null)
        {
            this.Title = Title.Create(title);
        }

        if (content is not null)
        {
            this.Content = content;
        }

        if (photos is not null)
        {
            this.photos.Clear();
            this.photos.AddRange(photos);
        }

        if (videos is not null)
        {
            this.videos.Clear();
            this.videos.AddRange(videos);
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a photo URL to the success story.
    /// </summary>
    /// <param name="photoUrl">The photo URL to add.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="photoUrl"/> is null or whitespace.</exception>
    public void AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new ArgumentException("URL фото не може бути порожнім.", nameof(photoUrl));
        }

        this.photos.Add(photoUrl);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a photo URL from the success story.
    /// </summary>
    /// <param name="photoUrl">The photo URL to remove.</param>
    public void RemovePhoto(string photoUrl)
    {
        if (this.photos.Remove(photoUrl))
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Adds a video URL to the success story.
    /// </summary>
    /// <param name="videoUrl">The video URL to add.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="videoUrl"/> is null or whitespace.</exception>
    public void AddVideo(string videoUrl)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            throw new ArgumentException("URL відео не може бути порожнім.", nameof(videoUrl));
        }

        this.videos.Add(videoUrl);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a video URL from the success story.
    /// </summary>
    /// <param name="videoUrl">The video URL to remove.</param>
    public void RemoveVideo(string videoUrl)
    {
        if (this.videos.Remove(videoUrl))
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
