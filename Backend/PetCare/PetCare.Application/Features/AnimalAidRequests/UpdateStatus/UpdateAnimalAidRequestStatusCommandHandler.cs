namespace PetCare.Application.Features.AnimalAidRequests.UpdateStatus;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles updating the status of an AnimalAidRequest.
/// </summary>
public sealed class UpdateAnimalAidRequestStatusCommandHandler
    : IRequestHandler<UpdateAnimalAidRequestStatusCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalAidRequestStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service for managing animal aid requests.</param>
    /// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="animalAidRequestService"/> or <paramref name="mapper"/> is null.
    /// </exception>
    public UpdateAnimalAidRequestStatusCommandHandler(
        IAnimalAidRequestService animalAidRequestService,
        IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        return this.mapper.Map<AnimalAidRequestDetailsDto>(updatedRequest);
    }
}
