namespace PetCare.Application.Features.Breeds.GetBreedsBySpecie;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve a list of breeds for a specified species.
/// </summary>
/// <remarks>This handler uses the provided species service to obtain breed data and maps the results to response
/// DTOs. It is typically used within a MediatR pipeline to process breed retrieval commands. Thread safety is ensured
/// by the stateless nature of the handler.</remarks>
public sealed class GetBreedsBySpecieCommandHandler : IRequestHandler<GetBreedsBySpecieCommand, GetBreedsResponseDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBreedsBySpecieCommandHandler"/> class with the specified specie service and.
    /// object mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve specie-related data required for breed queries.</param>
    /// <param name="mapper">The mapper used to convert domain entities to data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specieService"/> or <paramref name="mapper"/> is <see langword="null"/>.</exception>
    public GetBreedsBySpecieCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the request to retrieve a list of breeds for the specified species.
    /// </summary>
    /// <param name="request">The command containing the species identifier for which to retrieve breeds.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A response DTO containing the list of breeds and the total count for the specified species.</returns>
    public async Task<GetBreedsResponseDto> Handle(GetBreedsBySpecieCommand request, CancellationToken cancellationToken)
    {
        var breeds = await this.specieService.GetBreedsAsync(request.SpecieId, cancellationToken);
        var breedDtos = this.mapper.Map<IReadOnlyList<BreedListDto>>(breeds);
        return new GetBreedsResponseDto(breedDtos, breedDtos.Count);
    }
}
