namespace PetCare.Application.Features.Species.GetSpecies;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for GetSpeciesCommand.
/// </summary>
public sealed class GetSpeciesCommandHandler : IRequestHandler<GetSpeciesCommand, GetSpeciesResponseDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSpeciesCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve species data and perform related operations.</param>
    /// <param name="mapper">The mapper used to convert species domain models to data transfer objects.</param>
    public GetSpeciesCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Handles the retrieval of species data based on the specified command.
    /// </summary>
    /// <param name="request">The command containing parameters for the species retrieval operation.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a response DTO with the list of
    /// species.</returns>
    public async Task<GetSpeciesResponseDto> Handle(GetSpeciesCommand request, CancellationToken cancellationToken)
    {
        var species = await this.specieService.GetAllSpeciesAsync(cancellationToken);
        var dtos = this.mapper.Map<IReadOnlyList<SpecieListDto>>(species);
        return new GetSpeciesResponseDto(dtos);
    }
}
