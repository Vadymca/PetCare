namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing lost pet reports.
/// </summary>
public sealed class LostPetService : ILostPetService
{
    /// <inheritdoc/>
    public async Task AddLostPetAsync(User user, LostPet lostPet, Guid requestingUserId)
    {
        user.AddLostPet(lostPet, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveLostPetAsync(User user, Guid lostPetId, Guid requestingUserId)
    {
        user.RemoveLostPet(lostPetId, requestingUserId);
        await Task.CompletedTask;
    }
}
