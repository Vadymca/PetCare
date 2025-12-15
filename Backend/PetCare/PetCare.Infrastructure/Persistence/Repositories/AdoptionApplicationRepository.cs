namespace PetCare.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;

/// <summary>
/// Repository implementation for managing <see cref="AdoptionApplication"/> entities.
/// </summary>
public class AdoptionApplicationRepository : GenericRepository<AdoptionApplication>, IAdoptionApplicationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AdoptionApplicationRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplication>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<AdoptionApplication>()
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplication>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<AdoptionApplication>()
            .Where(a => a.Status == Domain.Enums.AdoptionStatus.Pending)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplication>> GetApprovedAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<AdoptionApplication>()
            .Where(a => a.Status == Domain.Enums.AdoptionStatus.Approved)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplication>> GetRejectedAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<AdoptionApplication>()
            .Where(a => a.Status == Domain.Enums.AdoptionStatus.Rejected)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task ApproveAsync(
        Guid applicationId,
        Guid adminId,
        string? curatorName = null,
        string? curatorPhone = null,
        CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        // Передаємо куратора у метод агрегату
        application.Approve(adminId, curatorName, curatorPhone);

        await this.UpdateAsync(application, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RejectAsync(Guid applicationId, string reason, CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        application.Reject(reason);
        await this.UpdateAsync(application, cancellationToken);
    }
}
