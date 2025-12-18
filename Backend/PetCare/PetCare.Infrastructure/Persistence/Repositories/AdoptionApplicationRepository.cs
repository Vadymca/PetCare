namespace PetCare.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

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
        DateTime? meetingDate = null,
        CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        if (application.Status != AdoptionStatus.Pending)
        {
            throw new InvalidOperationException("Можна підтвердити лише заявку зі статусом Pending.");
        }

        var animal = await this.Context.Set<Animal>()
            .FirstOrDefaultAsync(a => a.Id == application.AnimalId, cancellationToken);

        if (animal == null)
        {
            throw new InvalidOperationException("Тварину не знайдено.");
        }

        // Транзакційна обробка
        await using var transaction = await this.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Змінюємо статус заявки
            application.Approve(adminId, curatorName, curatorPhone, meetingDate);

            // Резервуємо тварину
            if (animal.Status != AnimalStatus.Reserved)
            {
                animal.ChangeStatus(AnimalStatus.Reserved);
            }

            await this.UpdateAsync(application, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task RejectAsync(Guid applicationId, string reason, CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        if (application.Status != AdoptionStatus.Pending)
        {
            throw new InvalidOperationException("Можна відхилити лише заявку зі статусом Pending.");
        }

        // Транзакційна обробка
        await using var transaction = await this.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Змінюємо статус заявки
            application.Reject(reason);

            // Відновлюємо тварину у доступний статус, якщо була заброньована для цієї заявки
            var animal = await this.Context.Set<Animal>()
                .FirstOrDefaultAsync(a => a.Id == application.AnimalId, cancellationToken);

            if (animal != null && animal.Status == AnimalStatus.Reserved)
            {
                animal.ChangeStatus(AnimalStatus.Available);
            }

            await this.UpdateAsync(application, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplication> CreateWithAnimalReservationAsync(
    Guid userId,
    Guid animalId,
    string? comment,
    CancellationToken cancellationToken = default)
    {
        var animal = await this.Context.Set<Animal>()
            .FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken);

        if (animal == null)
        {
            throw new InvalidOperationException("Тварину не знайдено.");
        }

        if (animal.Status == AnimalStatus.Reserved || animal.Status == AnimalStatus.Adopted)
        {
            throw new InvalidOperationException("Тварина вже зарезервована або усиновлена.");
        }

        var application = AdoptionApplication.Create(userId, animalId, comment);

        await using var transaction = await this.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Додаємо заявку
            await this.AddAsync(application, cancellationToken);

            // Міняємо статус тварини
            animal.ChangeStatus(AnimalStatus.Reserved);

            // Зберігаємо обидві зміни одночасно
            await this.Context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return application;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteWithAnimalReleaseAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        var animal = await this.Context.Set<Animal>()
            .FirstOrDefaultAsync(a => a.Id == application.AnimalId, cancellationToken);

        if (animal == null)
        {
            throw new InvalidOperationException("Тварину не знайдено.");
        }

        // Транзакційна обробка
        await using var transaction = await this.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Видаляємо заявку
            this.Context.Set<AdoptionApplication>().Remove(application);

            // Змінюємо статус тварини, якщо вона була зарезервована
            if (animal.Status == AnimalStatus.Reserved)
            {
                animal.ChangeStatus(AnimalStatus.Available);
            }

            await this.Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task CompleteAdoptionAsync(Guid applicationId, bool isAdopted, CancellationToken cancellationToken = default)
    {
        var application = await this.GetByIdAsync(applicationId, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        var animal = await this.Context.Set<Animal>()
            .FirstOrDefaultAsync(a => a.Id == application.AnimalId, cancellationToken);

        if (animal == null)
        {
            throw new InvalidOperationException("Тварину не знайдено.");
        }

        // Транзакція для обох змін
        await using var transaction = await this.Context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (isAdopted)
            {
                application.CompleteAdoption(); // встановлює AdoptionDate
                animal.ChangeStatus(AnimalStatus.Adopted);
            }
            else
            {
                // Якщо усиновлення не відбулося
                animal.ChangeStatus(AnimalStatus.Available);
            }

            await this.Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
