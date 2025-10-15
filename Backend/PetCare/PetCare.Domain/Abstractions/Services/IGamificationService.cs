namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Defines asynchronous operations for managing user points and gamification rewards.
/// </summary>
public interface IGamificationService
{
    /// <summary>
    /// Adds points to a user account. Only administrators can perform this action.
    /// </summary>
    /// <param name="user">The user to whom points will be added.</param>
    /// <param name="amount">The number of points to add.</param>
    /// <param name="requestingUserId">The identifier of the user performing the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddPointsAsync(User user, int amount, Guid requestingUserId);

    /// <summary>
    /// Deducts points from a user account. Only administrators can perform this action.
    /// </summary>
    /// <param name="user">The user from whom points will be deducted.</param>
    /// <param name="amount">The number of points to deduct.</param>
    /// <param name="requestingUserId">The identifier of the user performing the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeductPointsAsync(User user, int amount, Guid requestingUserId);

    /// <summary>
    /// Adds a gamification reward to a user. Only administrators can perform this action.
    /// </summary>
    /// <param name="user">The user who will receive the reward.</param>
    /// <param name="reward">The reward to add.</param>
    /// <param name="requestingUserId">The identifier of the user performing the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddGamificationRewardAsync(User user, GamificationReward reward, Guid requestingUserId);

    /// <summary>
    /// Removes a gamification reward from a user. Administrators and moderators can perform this action.
    /// </summary>
    /// <param name="user">The user whose reward will be removed.</param>
    /// <param name="rewardId">The identifier of the reward to remove.</param>
    /// <param name="requestingUserId">The identifier of the user performing the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the reward was successfully removed, otherwise <c>false</c>.
    /// </returns>
    Task<bool> RemoveGamificationRewardAsync(User user, Guid rewardId, Guid requestingUserId);
}
