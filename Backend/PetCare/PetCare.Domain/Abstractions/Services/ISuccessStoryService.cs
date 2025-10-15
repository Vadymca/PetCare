namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Domain service for managing user success stories.
/// </summary>
public interface ISuccessStoryService
{
    /// <summary>
    /// Adds a new success story created by the user.
    /// </summary>
    /// <param name="user">The user aggregate creating the story.</param>
    /// <param name="story">The success story to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID or have elevated rights.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddSuccessStoryAsync(User user, SuccessStory story, Guid requestingUserId);

    /// <summary>
    /// Removes a success story from the user's collection. Only admins or moderators can perform this action.
    /// </summary>
    /// <param name="user">The user aggregate from which the story will be removed.</param>
    /// <param name="storyId">The unique identifier of the success story to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <returns>True if the story was removed; otherwise, false.</returns>
    Task<bool> RemoveSuccessStoryAsync(User user, Guid storyId, Guid requestingUserId);
}
