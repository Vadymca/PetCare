namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Persistence.Repositories;

/// <summary>
/// Provides methods to manage <see cref="AnimalAidRequest"/> entities through the <see cref="ShelterRepository"/>.
/// </summary>
public sealed class AnimalAidRequestService : IAnimalAidRequestService
{
    private readonly IShelterRepository shelterRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalAidRequestService"/> class.
    /// </summary>
    /// <param name="shelterRepository">The shelter repository to use for data access.</param>
    public AnimalAidRequestService(IShelterRepository shelterRepository)
    {
        this.shelterRepository = shelterRepository ?? throw new ArgumentNullException(nameof(shelterRepository));
    }

    /// <inheritdoc/>
    public Task<List<AnimalAidRequest>> GetAllAnimalAidRequestsAsync(CancellationToken cancellationToken = default)
        => this.shelterRepository.GetAllAnimalAidRequestsAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<AnimalAidRequest> GetAnimalAidRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => this.shelterRepository.GetAnimalAidRequestByIdAsync(id, cancellationToken);

    /// <inheritdoc/>
    public Task<AnimalAidRequest> GetAnimalAidRequestBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => this.shelterRepository.GetAnimalAidRequestBySlugAsync(slug, cancellationToken);

    /// <inheritdoc/>
    public Task<AnimalAidRequest> CreateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default)
        => this.shelterRepository.CreateAnimalAidRequestAsync(request, cancellationToken);

    /// <inheritdoc/>
    public Task UpdateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default)
        => this.shelterRepository.UpdateAnimalAidRequestAsync(request, cancellationToken);

    /// <inheritdoc/>
    public Task UpdateAnimalAidRequestStatusAsync(Guid id, AidStatus status, CancellationToken cancellationToken = default)
        => this.shelterRepository.UpdateAnimalAidRequestStatusAsync(id, status, cancellationToken);

    /// <inheritdoc/>
    public Task DeleteAnimalAidRequestAsync(Guid id, CancellationToken cancellationToken = default)
        => this.shelterRepository.DeleteAnimalAidRequestAsync(id, cancellationToken);

    /// <inheritdoc/>
    public Task<List<AnimalAidRequest>> GetUrgentAnimalAidRequestsAsync(CancellationToken cancellationToken = default)
        => this.shelterRepository.GetUrgentAnimalAidRequestsAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task AttachDonationAsync(Guid aidRequestId, Guid donationId, CancellationToken cancellationToken = default)
    {
        var request = await this.shelterRepository.GetAnimalAidRequestByIdAsync(aidRequestId, cancellationToken)
            ?? throw new InvalidOperationException($"AnimalAidRequest з Id '{aidRequestId}' не знайдено.");

        request.AttachDonation(donationId);

        await this.shelterRepository.UpdateAnimalAidRequestAsync(request, cancellationToken);
    }
}
