namespace PetCare.Application.Features.Species.UpdateSpecie;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to update an existing species and returns the updated species details.
/// </summary>
/// <remarks>This handler coordinates the update operation by invoking the species service and mapping the result
/// to a data transfer object. It is typically used within a MediatR pipeline to process update commands for species
/// entities.</remarks>
public sealed class UpdateSpecieCommandHandler : IRequestHandler<UpdateSpecieCommand, SpecieDetailDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSpecieCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to perform operations related to species data.</param>
    /// <param name="mapper">The mapper used to convert between domain models and data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if either specieService or mapper is null.</exception>
    public UpdateSpecieCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the update operation for a species and returns the updated species details.
    /// </summary>
    /// <param name="request">The command containing the species identifier and updated information to apply.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpecieDetailDto"/> with
    /// the updated species details.</returns>
    public async Task<SpecieDetailDto> Handle(UpdateSpecieCommand request, CancellationToken cancellationToken)
    {
        var updatedSpecie = await this.specieService.UpdateSpeciesAsync(request.Id, request.Name, cancellationToken);

        return this.mapper.Map<SpecieDetailDto>(updatedSpecie);
    }
}
