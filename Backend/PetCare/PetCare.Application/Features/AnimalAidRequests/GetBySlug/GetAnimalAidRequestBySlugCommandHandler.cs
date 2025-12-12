namespace PetCare.Application.Features.AnimalAidRequests.GetBySlug;

using System;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAnimalAidRequestBySlugCommand"/> requests.
/// </summary>
public sealed class GetAnimalAidRequestBySlugCommandHandler : IRequestHandler<GetAnimalAidRequestBySlugCommand, AnimalAidRequestDetailsDto?>
{
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalAidRequestBySlugCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">The service used to retrieve animal aid request data.</param>
    /// <param name="mapper">The mapper used to convert entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the dependencies are null.</exception>
    public GetAnimalAidRequestBySlugCommandHandler(IAnimalAidRequestService animalAidRequestService, IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AnimalAidRequestDetailsDto?> Handle(GetAnimalAidRequestBySlugCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(request.Slug));
        }

        var aidRequest = await this.animalAidRequestService.GetAnimalAidRequestBySlugAsync(request.Slug, cancellationToken)
            ?? throw new InvalidOperationException($"Запит на допомогу зі slug '{request.Slug}' не знайдено.");

        return this.mapper.Map<AnimalAidRequestDetailsDto>(aidRequest);
    }
}
