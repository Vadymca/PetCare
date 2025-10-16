namespace PetCare.Application.Features.Shelters.GetShelters;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetSheltersCommand"/>.
/// </summary>
public sealed class GetSheltersCommandHandler : IRequestHandler<GetSheltersCommand, GetSheltersResponseDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSheltersCommandHandler"/> class with the specified shelter service and object.
    /// mapper.
    /// </summary>
    /// <param name="shelterService">The service used to retrieve shelter data.</param>
    /// <param name="mapper">The mapper used to convert shelter entities to data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shelterService"/> or <paramref name="mapper"/> is null.</exception>
    public GetSheltersCommandHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<GetSheltersResponseDto> Handle(GetSheltersCommand request, CancellationToken cancellationToken)
    {
        var (shelters, total) = await this.shelterService.GetSheltersAsync(request.Page, request.PageSize, cancellationToken);
        var shelterDtos = this.mapper.Map<IReadOnlyList<ShelterListDto>>(shelters);
        return new GetSheltersResponseDto(shelterDtos, total);
    }
}
