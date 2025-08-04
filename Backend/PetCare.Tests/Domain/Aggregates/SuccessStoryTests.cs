// <copyright file="SuccessStoryTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using PetCare.Domain.Aggregates;
using System;
using System.Collections.Generic;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="SuccessStory"/> aggregate.
/// </summary>
public sealed class SuccessStoryTests
{
    /// <summary>
    /// Verifies that a valid success story is created with correct defaults.
    /// </summary>
    [Fact]
    public void Create_WithValidData_ShouldInitializeProperly()
    {
        var story = CreateTestStory();

        Assert.Equal("REUNITED!", story.Title.Value.ToUpperInvariant());
        Assert.Equal("After a long journey...", story.Content);
        Assert.Single(story.Photos);
        Assert.Single(story.Videos);
        Assert.Equal(0, story.Views);
        Assert.InRange(story.CreatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(story.PublishedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies that creating a story with empty animal ID throws.
    /// </summary>
    [Fact]
    public void Create_WithEmptyAnimalId_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            SuccessStory.Create(Guid.Empty, Guid.NewGuid(), "title", "content"));
    }

    /// <summary>
    /// Verifies that creating a story with empty content throws.
    /// </summary>
    [Fact]
    public void Create_WithEmptyContent_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            SuccessStory.Create(Guid.NewGuid(), Guid.NewGuid(), "title", string.Empty));
    }

    /// <summary>
    /// Verifies that updating title and content replaces old values.
    /// </summary>
    [Fact]
    public void Update_WithNewTitleAndContent_ShouldReplaceThem()
    {
        var story = CreateTestStory();
        var newTitle = "New Title";
        var newContent = "Updated content.";

        story.Update(title: newTitle, content: newContent);

        Assert.Equal(newTitle, story.Title.Value);
        Assert.Equal(newContent, story.Content);
    }

    /// <summary>
    /// Verifies that updating photos and videos replaces the collections.
    /// </summary>
    [Fact]
    public void Update_WithNewPhotosAndVideos_ShouldReplaceCollections()
    {
        var story = CreateTestStory();
        var photos = new List<string> { "new1.jpg", "new2.jpg" };
        var videos = new List<string> { "new1.mp4" };

        story.Update(photos: photos, videos: videos);

        Assert.Equal(2, story.Photos.Count);
        Assert.Single(story.Videos);
    }

    /// <summary>
    /// Verifies that adding a valid photo adds to the collection.
    /// </summary>
    [Fact]
    public void AddPhoto_WithValidUrl_ShouldAdd()
    {
        var story = CreateTestStory();
        var url = "new.jpg";

        story.AddPhoto(url);

        Assert.Contains(url, story.Photos);
    }

    /// <summary>
    /// Verifies that adding an empty photo URL throws.
    /// </summary>
    [Fact]
    public void AddPhoto_WithEmptyUrl_ShouldThrow()
    {
        var story = CreateTestStory();

        Assert.Throws<ArgumentException>(() => story.AddPhoto("  "));
    }

    /// <summary>
    /// Verifies that removing a photo deletes it from collection.
    /// </summary>
    [Fact]
    public void RemovePhoto_IfExists_ShouldRemove()
    {
        var story = CreateTestStory();
        var url = story.Photos[0];

        story.RemovePhoto(url);

        Assert.DoesNotContain(url, story.Photos);
    }

    /// <summary>
    /// Verifies that adding a valid video adds to the collection.
    /// </summary>
    [Fact]
    public void AddVideo_WithValidUrl_ShouldAdd()
    {
        var story = CreateTestStory();
        var url = "new.mp4";

        story.AddVideo(url);

        Assert.Contains(url, story.Videos);
    }

    /// <summary>
    /// Verifies that adding an empty video URL throws.
    /// </summary>
    [Fact]
    public void AddVideo_WithEmptyUrl_ShouldThrow()
    {
        var story = CreateTestStory();

        Assert.Throws<ArgumentException>(() => story.AddVideo(string.Empty));
    }

    /// <summary>
    /// Verifies that removing a video deletes it from collection.
    /// </summary>
    [Fact]
    public void RemoveVideo_IfExists_ShouldRemove()
    {
        var story = CreateTestStory();
        var url = story.Videos[0];

        story.RemoveVideo(url);

        Assert.DoesNotContain(url, story.Videos);
    }

    /// <summary>
    /// Verifies that view counter increments properly.
    /// </summary>
    [Fact]
    public void IncrementViews_ShouldIncreaseByOne()
    {
        var story = CreateTestStory();
        var viewsBefore = story.Views;

        story.IncrementViews();

        Assert.Equal(viewsBefore + 1, story.Views);
    }

    private static SuccessStory CreateTestStory()
    {
        return SuccessStory.Create(
            animalId: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Reunited!",
            content: "After a long journey...",
            photos: new List<string> { "p1.jpg" },
            videos: new List<string> { "v1.mp4" });
    }
}
