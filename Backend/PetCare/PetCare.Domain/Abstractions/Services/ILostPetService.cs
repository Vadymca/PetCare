namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides asynchronous operations for managing lost pet reports.
/// </summary>
public interface ILostPetService
{
    /// <summary>
    /// Adds a lost pet report to the user's collection.
    /// </summary>
    /// <param name="user">The user who owns the lost pet report.</param>
    /// <param name="lostPet">The lost pet report to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddLostPetAsync(User user, LostPet lostPet, Guid requestingUserId);

    /// <summary>
    /// Removes a lost pet report from the user's collection.
    /// </summary>
    /// <param name="user">The user who owns the lost pet report.</param>
    /// <param name="lostPetId">The ID of the lost pet report to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveLostPetAsync(User user, Guid lostPetId, Guid requestingUserId);
}
