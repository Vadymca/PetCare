namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;

/// <summary>
/// Provides functionality for managing adoption applications,
/// including adding, removing, and updating their status.
/// </summary>
public sealed class AdoptionApplicationService : IAdoptionApplicationService
{
    /// <inheritdoc/>
    public async Task AddAdoptionApplicationAsync(User user, AdoptionApplication application, Guid requestingUserId)
    {
        user.AddAdoptionApplication(application, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveAdoptionApplicationAsync(User user, Guid applicationId, Guid requestingUserId)
    {
        var removed = user.RemoveAdoptionApplication(applicationId, requestingUserId);
        return await Task.FromResult(removed);
    }
}
