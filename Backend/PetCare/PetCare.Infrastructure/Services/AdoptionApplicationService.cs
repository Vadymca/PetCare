namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Implementation of the adoption application service.
/// </summary>
public sealed class AdoptionApplicationService : IAdoptionApplicationService
{
    private readonly IAdoptionApplicationRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationService"/> class.
    /// </summary>
    /// <param name="repository">The adoption application repository.</param>
    public AdoptionApplicationService(IAdoptionApplicationRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplication> CreateAsync(Guid userId, Guid animalId, string? comment, CancellationToken cancellationToken = default)
    {
        var application = AdoptionApplication.Create(userId, animalId, comment);
        return await this.repository.AddAsync(application, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplication> UpdateAsync(
    Guid id,
    string? comment,
    string? adminNotes,
    CancellationToken cancellationToken = default)
    {
        var application = await this.repository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("Заявку не знайдено.");

        if (!string.IsNullOrWhiteSpace(comment))
        {
            application.UpdateComment(comment);
        }

        if (!string.IsNullOrWhiteSpace(adminNotes))
        {
            application.AddAdminNotes(adminNotes);
        }

        return await this.repository.UpdateAsync(application, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var application = await this.repository.GetByIdAsync(id, cancellationToken) ?? throw new InvalidOperationException("Заявку не знайдено.");
        await this.repository.DeleteAsync(application, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplication> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var application = await this.repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
        {
            throw new InvalidOperationException("Заявку не знайдено.");
        }

        return application;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<AdoptionApplication>> GetAllAsync(CancellationToken cancellationToken = default)
        => this.repository.GetAllAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<IReadOnlyList<AdoptionApplication>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => this.repository.GetByUserAsync(userId, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplication>> GetByStatusAsync(AdoptionStatus status, CancellationToken cancellationToken = default)
    {
        return status switch
        {
            AdoptionStatus.Pending => await this.repository.GetPendingAsync(cancellationToken),
            AdoptionStatus.Approved => await this.repository.GetApprovedAsync(cancellationToken),
            AdoptionStatus.Rejected => await this.repository.GetRejectedAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(status), "Невідомий статус заявки."),
        };
    }

    /// <inheritdoc/>
    public Task ApproveAsync(
        Guid id,
        Guid adminId,
        string? curatorName = null,
        string? curatorPhone = null,
        CancellationToken cancellationToken = default)
        {
            return this.repository.ApproveAsync(id, adminId, curatorName, curatorPhone, cancellationToken);
        }

    /// <inheritdoc/>
    public Task RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default)
        => this.repository.RejectAsync(id, reason, cancellationToken);
}
