namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides asynchronous operations for managing donations made by users.
/// </summary>
public interface IDonationService
{
    /// <summary>
    /// Adds a new donation made by the user.
    /// </summary>
    /// <param name="user">The user making the donation.</param>
    /// <param name="donation">The donation to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddDonationAsync(User user, Donation donation, Guid requestingUserId);

    /// <summary>
    /// Removes a donation made by the user.
    /// </summary>
    /// <param name="user">The user who owns the donation.</param>
    /// <param name="donationId">The ID of the donation to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveDonationAsync(User user, Guid donationId, Guid requestingUserId);
}
