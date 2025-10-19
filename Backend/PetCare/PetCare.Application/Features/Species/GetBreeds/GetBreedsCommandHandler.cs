namespace PetCare.Application.Features.Species.GetBreeds;

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
/// <remarks>This handler coordinates fetching breed data from the species service and mapping it to response
/// DTOs. It is typically used within a MediatR pipeline to process GetBreedsCommand requests. The handler is
/// thread-safe and intended for use in dependency injection scenarios.</remarks>
public sealed class GetBreedsCommandHandler : IRequestHandler<GetBreedsCommand, GetBreedsResponseDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBreedsCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve specie-related data required for breed operations. Cannot be null.</param>
    /// <param name="mapper">The mapper used to convert domain entities to data transfer objects. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specieService"/> or <paramref name="mapper"/> is null.</exception>
    public GetBreedsCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the request to retrieve a list of breeds for the specified species.
    /// </summary>
    /// <param name="request">The command containing the species identifier for which to retrieve breeds. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A response object containing the list of breeds and the total count for the specified species.</returns>
    public async Task<GetBreedsResponseDto> Handle(GetBreedsCommand request, CancellationToken cancellationToken)
    {
        var breeds = await this.specieService.GetBreedsAsync(request.SpecieId, cancellationToken);
        var breedDtos = this.mapper.Map<IReadOnlyList<BreedListDto>>(breeds);

        return new GetBreedsResponseDto(breedDtos, breedDtos.Count);
    }
}
