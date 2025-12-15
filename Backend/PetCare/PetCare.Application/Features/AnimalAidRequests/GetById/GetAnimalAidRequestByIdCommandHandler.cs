namespace PetCare.Application.Features.AnimalAidRequests.GetById;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAnimalAidRequestByIdCommand"/> requests.
/// </summary>
public sealed class GetAnimalAidRequestByIdCommandHandler
    : IRequestHandler<GetAnimalAidRequestByIdCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalAidRequestByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to retrieve animal aid request data.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the dependencies are null.</exception>
    public GetAnimalAidRequestByIdCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto> Handle(
        GetAnimalAidRequestByIdCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Id не може бути порожнім.", nameof(request.Id));
        }

        var aidRequest = await this.animalAidRequestService.GetAnimalAidRequestByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Запит на допомогу з Id '{request.Id}' не знайдено.");

        // отримуємо суму зібраних пожертв
        var donatedAmount = await this.animalAidRequestService.GetCollectedAmountAsync(aidRequest.Id, cancellationToken);

        var donationsCount = await this.animalAidRequestService.GetDonationsCountAsync(aidRequest.Id, cancellationToken);

        // ручне мапування в DTO
        var dto = new AnimalAidRequestDetailsDto(
            aidRequest.Id,
            aidRequest.Slug.Value,
            aidRequest.Shelter != null ? new ShelterInfoDto(aidRequest.Shelter.Id, aidRequest.Shelter.Name.Value, aidRequest.Shelter.Slug.Value) : null,
            aidRequest.Title.Value,
            aidRequest.Description ?? string.Empty,
            aidRequest.Category,
            aidRequest.EstimatedCost ?? 0m,
            donatedAmount,
            donationsCount,
            aidRequest.Status,
            aidRequest.Photos,
            aidRequest.CreatedAt);

        return dto;
    }
}
