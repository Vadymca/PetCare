namespace PetCare.Application.Features.AnimalAidRequests.GetById;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAnimalAidRequestByIdCommand"/> requests.
/// </summary>
public sealed class GetAnimalAidRequestByIdCommandHandler
    : IRequestHandler<GetAnimalAidRequestByIdCommand, AnimalAidRequestDetailsDto>
{
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalAidRequestByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to retrieve animal aid request data.</param>
    /// <param name="mapper">The mapper used to convert entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the dependencies are null.</exception>
    public GetAnimalAidRequestByIdCommandHandler(
        IAnimalAidRequestService animalAidRequestService,
        IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        return this.mapper.Map<AnimalAidRequestDetailsDto>(aidRequest);
    }
}
