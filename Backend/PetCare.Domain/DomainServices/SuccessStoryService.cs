namespace PetCare.Domain.DomainServices;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Implementation of the domain service for managing success stories.
/// </summary>
public sealed class SuccessStoryService : ISuccessStoryService
{
    /// <inheritdoc/>
    public async Task AddSuccessStoryAsync(User user, SuccessStory story, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (story is null)
        {
            throw new ArgumentNullException(nameof(story));
        }

        user.AddSuccessStory(story, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveSuccessStoryAsync(User user, Guid storyId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var story = user.SuccessStories.FirstOrDefault(s => s.Id == storyId);
        if (story == null)
        {
            return await Task.FromResult(false);
        }

        user.RemoveSuccessStoryAsAdmin(storyId, requestingUserId);

        return await Task.FromResult(true);
    }
}
