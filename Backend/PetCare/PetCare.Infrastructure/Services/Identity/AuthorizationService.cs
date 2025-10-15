namespace PetCare.Infrastructure.Services.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Provides authorization checks using ASP.NET Identity and EF Core.
/// </summary>
public class AuthorizationService(
    AppDbContext dbContext,
    UserManager<User> userManager)
    : IAuthorizationService
{
    /// <inheritdoc/>
    public async Task<bool> CanAccessShelterAsync(Guid userId, Guid shelterId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return false;
        }

        if (await userManager.IsInRoleAsync(user, Role.Admin.ToString()))
        {
            return true;
        }

        var isManager = await dbContext.Shelters
            .AnyAsync(s => s.Id == shelterId && s.ManagerId == userId, cancellationToken);

        return isManager;
    }

    /// <inheritdoc/>
    public async Task<bool> CanManageAnimalAsync(Guid userId, Guid animalId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return false;
        }

        if (await userManager.IsInRoleAsync(user, Role.Admin.ToString()))
        {
            return true;
        }

        var isManager = await dbContext.Animals
            .Include(a => a.Shelter)
            .AnyAsync(a => a.Id == animalId && a.Shelter!.ManagerId == userId, cancellationToken);

        return isManager;
    }

    /// <inheritdoc/>
    public async Task<bool> HasRoleAsync(Guid userId, Role role, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return false;
        }

        return await userManager.IsInRoleAsync(user, role.ToString());
    }
}
