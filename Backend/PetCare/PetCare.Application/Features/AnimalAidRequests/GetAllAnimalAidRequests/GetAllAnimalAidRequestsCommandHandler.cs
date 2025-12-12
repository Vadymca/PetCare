namespace PetCare.Application.Features.AnimalAidRequests.GetAllAnimalAidRequests;

using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAllAnimalAidRequestsCommand"/> requests.
/// </summary>
public sealed class GetAllAnimalAidRequestsCommandHandler
    : IRequestHandler<GetAllAnimalAidRequestsCommand, IReadOnlyList<AnimalAidRequestListDto>>
{
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllAnimalAidRequestsCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">Service used to access animal aid request data.</param>
    /// <param name="mapper">Mapper to convert domain entities to DTOs.</param>
    public GetAllAnimalAidRequestsCommandHandler(
        IAnimalAidRequestService animalAidRequestService,
        IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService ?? throw new ArgumentNullException(nameof(animalAidRequestService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AnimalAidRequestListDto>> Handle(
        GetAllAnimalAidRequestsCommand request,
        CancellationToken cancellationToken)
    {
        var requests = await this.animalAidRequestService.GetAllAnimalAidRequestsAsync(cancellationToken);
        return this.mapper.Map<IReadOnlyList<AnimalAidRequestListDto>>(requests);
    }
}
