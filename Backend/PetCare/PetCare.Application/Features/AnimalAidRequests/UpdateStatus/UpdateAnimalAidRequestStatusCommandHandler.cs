namespace PetCare.Application.Features.AnimalAidRequests.UpdateStatus;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles updating the status of an AnimalAidRequest.
/// </summary>
public sealed class UpdateAnimalAidRequestStatusCommandHandler
    : IRequestHandler<UpdateAnimalAidRequestStatusCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalAidRequestStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service for managing animal aid requests.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="animalAidRequestService"/> or <paramref name="mapper"/> is null.
    /// </exception>
    public UpdateAnimalAidRequestStatusCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto> Handle(UpdateAnimalAidRequestStatusCommand request, CancellationToken cancellationToken)
    {
        // Використовуємо новий метод сервісу, який оновлює статус без завантаження повного об'єкта
        await this.animalAidRequestService.UpdateAnimalAidRequestStatusAsync(request.Id, request.Status, cancellationToken);

        // Після оновлення статусу отримуємо актуальний об'єкт для повернення DTO
        var updatedRequest = await this.animalAidRequestService.GetAnimalAidRequestByIdAsync(request.Id, cancellationToken);

        if (updatedRequest == null)
        {
            throw new InvalidOperationException("Запит на допомогу не знайдено після оновлення статусу.");
        }

        var collectedAmount = await this.animalAidRequestService.GetCollectedAmountAsync(
           updatedRequest.Id, cancellationToken);

        var dto = new AnimalAidRequestDetailsDto(
            updatedRequest.Id,
            updatedRequest.Slug.Value,
            updatedRequest.Shelter != null ? new ShelterInfoDto(updatedRequest.Shelter.Id, updatedRequest.Shelter.Name.Value, updatedRequest.Shelter.Slug.Value) : null,
            updatedRequest.Title.Value,
            updatedRequest.Description ?? string.Empty,
            updatedRequest.Category,
            updatedRequest.EstimatedCost ?? 0m,
            collectedAmount,
            updatedRequest.Status,
            updatedRequest.Photos.ToList(),
            updatedRequest.CreatedAt);

        return dto;
    }
}
