namespace PetCare.Domain.DomainServices;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using System;
using System.Threading.Tasks;

/// <summary>
/// Provides operations for managing donations made by users.
/// </summary>
public sealed class DonationService : IDonationService
{
    /// <inheritdoc/>
    public async Task AddDonationAsync(User user, Donation donation, Guid requestingUserId)
    {
        user.AddDonation(donation, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveDonationAsync(User user, Guid donationId, Guid requestingUserId)
    {
        user.RemoveDonation(donationId, requestingUserId);
        await Task.CompletedTask;
    }
}
