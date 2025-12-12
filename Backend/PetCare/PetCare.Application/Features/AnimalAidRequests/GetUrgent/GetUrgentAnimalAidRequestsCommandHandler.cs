namespace PetCare.Application.Features.AnimalAidRequests.GetUrgent;

using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles retrieving all urgent animal aid requests including donation summaries.
/// </summary>
public sealed class GetUrgentAnimalAidRequestsCommandHandler
    : IRequestHandler<GetUrgentAnimalAidRequestsCommand, List<UrgentAnimalAidRequestDto>>
{
    private readonly IAnimalAidRequestService animalAidRequestService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUrgentAnimalAidRequestsCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">Service used to access animal aid request data.</param>
    /// <param name="mapper">Instance of AutoMapper for converting entities to DTOs.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="animalAidRequestService"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public GetUrgentAnimalAidRequestsCommandHandler(
        IAnimalAidRequestService animalAidRequestService,
        IMapper mapper)
    {
        this.animalAidRequestService = animalAidRequestService
            ?? throw new ArgumentNullException(nameof(animalAidRequestService));

        this.mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the retrieval of urgent animal aid requests.
    /// </summary>
    /// <param name="request">The query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of urgent aid requests with donation statistics.</returns>
    public async Task<List<UrgentAnimalAidRequestDto>> Handle(
        GetUrgentAnimalAidRequestsCommand request,
        CancellationToken cancellationToken)
    {
        var urgentRequests = await this.animalAidRequestService
            .GetUrgentAnimalAidRequestsAsync(cancellationToken)
            .ConfigureAwait(false);

        return this.mapper.Map<List<UrgentAnimalAidRequestDto>>(urgentRequests);
    }
}
