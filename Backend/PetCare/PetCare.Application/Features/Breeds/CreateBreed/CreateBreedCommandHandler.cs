namespace PetCare.Application.Features.Breeds.CreateBreed;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the creation of a new breed and returns the resulting breed with its associated species information.
/// </summary>
/// <remarks>This handler processes a <see cref="CreateBreedCommand"/> by delegating breed creation to the
/// provided <see cref="ISpecieService"/> and mapping the result to a <see cref="BreedWithSpecieDto"/>. Instances of
/// this class are typically used within a MediatR pipeline to encapsulate the logic for creating breeds in the
/// application domain.</remarks>
public sealed class CreateBreedCommandHandler : IRequestHandler<CreateBreedCommand, BreedWithSpecieDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBreedCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve and manage specie information.</param>
    /// <param name="mapper">The mapper used to convert between domain models and data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if specieService or mapper is null.</exception>
    public CreateBreedCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Creates a new breed for the specified species and returns the resulting breed with its associated species
    /// information.
    /// </summary>
    /// <param name="request">The command containing the species identifier, breed name, and description to use when creating the new breed.
    /// Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a data transfer object with details
    /// of the created breed and its associated species.</returns>
    public async Task<BreedWithSpecieDto> Handle(CreateBreedCommand request, CancellationToken cancellationToken)
    {
        var breed = await this.specieService.AddBreedAsync(
            request.SpecieId,
            request.Name,
            request.Description,
            cancellationToken);

        return this.mapper.Map<BreedWithSpecieDto>(breed);
    }
}
