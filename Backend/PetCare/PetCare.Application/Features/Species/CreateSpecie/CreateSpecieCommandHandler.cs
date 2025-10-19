namespace PetCare.Application.Features.Species.CreateSpecie;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="CreateSpecieCommand"/>.
/// </summary>
public sealed class CreateSpecieCommandHandler : IRequestHandler<CreateSpecieCommand, SpecieListDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSpecieCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to perform operations related to species.</param>
    /// <param name="mapper">The mapper used to convert between domain models and data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if either specieService or mapper is null.</exception>
    public CreateSpecieCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Creates a new species using the provided command and returns a data transfer object representing the created
    /// species.
    /// </summary>
    /// <param name="request">The command containing the details of the species to create. Must not be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="SpecieListDto"/> representing the newly created species.</returns>
    public async Task<SpecieListDto> Handle(CreateSpecieCommand request, CancellationToken cancellationToken)
    {
        var specie = await this.specieService.CreateSpeciesAsync(request.Name, cancellationToken);
        return this.mapper.Map<SpecieListDto>(specie);
    }
}
