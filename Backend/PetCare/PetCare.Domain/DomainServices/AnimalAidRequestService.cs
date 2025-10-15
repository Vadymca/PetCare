namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Service for managing animal aid requests.
/// </summary>
public sealed class AnimalAidRequestService : IAnimalAidRequestService
{
    /// <inheritdoc/>
    public async Task AddAnimalAidRequestAsync(User user, AnimalAidRequest request, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Користувач не може бути null.");
        }

        user.AddAnimalAidRequest(request, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveAnimalAidRequestAsAdminAsync(User user, Guid requestId, Guid requestingUserId)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Користувач не може бути null.");
        }

        user.RemoveAnimalAidRequestAsAdmin(requestId, requestingUserId);
        await Task.CompletedTask;
    }
}
