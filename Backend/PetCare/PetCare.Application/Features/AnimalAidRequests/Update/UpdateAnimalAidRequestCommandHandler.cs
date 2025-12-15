namespace PetCare.Application.Features.AnimalAidRequests.Update;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles updating an existing <see cref="AnimalAidRequest"/>.
/// </summary>
public sealed class UpdateAnimalAidRequestCommandHandler
    : IRequestHandler<UpdateAnimalAidRequestCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalAidRequestCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service for managing animal aid requests.</param>
    public UpdateAnimalAidRequestCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto> Handle(
        UpdateAnimalAidRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Retrieve existing request
        var aidRequest = await this.animalAidRequestService.GetAnimalAidRequestByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Запит на допомогу не знайдено.");

        // Update fields only if they are not null
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            aidRequest.UpdateTitle(request.Title);
        }

        if (!string.IsNullOrWhiteSpace(request.ShortDescription))
        {
            aidRequest.UpdateShortDescription(request.ShortDescription);
        }

        if (request.Description is not null)
        {
            aidRequest.UpdateDescription(request.Description);
        }

        if (request.Category is not null)
        {
            aidRequest.UpdateCategory(Enum.Parse<AidCategory>(request.Category));
        }

        if (request.EstimatedCost.HasValue)
        {
            aidRequest.UpdateEstimatedCost(request.EstimatedCost);
        }

        if (request.Status.HasValue)
        {
            aidRequest.UpdateStatus(request.Status.Value);
        }

        if (request.ContactPhone is not null)
        {
            aidRequest.UpdateContactPhone(request.ContactPhone);
        }

        if (request.IsUrgent.HasValue)
        {
            aidRequest.SetUrgency(request.IsUrgent.Value);
        }

        if (request.Photos != null)
        {
            foreach (var existingPhoto in aidRequest.Photos.ToList())
            {
                aidRequest.RemovePhoto(existingPhoto);
            }

            foreach (var photo in request.Photos)
            {
                aidRequest.AddPhoto(photo);
            }
        }

        await this.animalAidRequestService.UpdateAnimalAidRequestAsync(aidRequest, cancellationToken);

        var collectedAmount = await this.animalAidRequestService.GetCollectedAmountAsync(aidRequest.Id, cancellationToken);

        var donationsCount = await this.animalAidRequestService.GetDonationsCountAsync(aidRequest.Id, cancellationToken);

        var dto = new AnimalAidRequestDetailsDto(
            aidRequest.Id,
            aidRequest.Slug.Value,
            aidRequest.Shelter != null ? new ShelterInfoDto(aidRequest.Shelter.Id, aidRequest.Shelter.Name.Value, aidRequest.Shelter.Slug.Value) : null,
            aidRequest.Title.Value,
            aidRequest.Description ?? string.Empty,
            aidRequest.Category,
            aidRequest.EstimatedCost ?? 0m,
            collectedAmount,
            donationsCount,
            aidRequest.Status,
            aidRequest.Photos.ToList(),
            aidRequest.CreatedAt);

        return dto;
    }
}
