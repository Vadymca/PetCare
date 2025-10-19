namespace PetCare.Application.Features.Breeds.GetBreedById;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve a breed by its identifier and returns the breed along with its associated species
/// information.
/// </summary>
/// <remarks>This handler is typically used in a CQRS (Command Query Responsibility Segregation) pattern to
/// process queries for breed details. It relies on an implementation of ISpecieService to access breed data and uses
/// IMapper to map domain entities to data transfer objects. The handler is designed to be used with MediatR's
/// request/response pipeline.</remarks>
public sealed class GetBreedByIdCommandHandler : IRequestHandler<GetBreedByIdCommand, BreedWithSpecieDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBreedByIdCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve specie-related data required by the command handler. Cannot be null.</param>
    /// <param name="mapper">The object mapper used to map data models to response objects. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if either specieService or mapper is null.</exception>
    public GetBreedByIdCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves a breed and its associated species information based on the specified command.
    /// </summary>
    /// <param name="request">The command containing the identifier of the breed to retrieve. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a data transfer object with breed
    /// and species information.</returns>
    public async Task<BreedWithSpecieDto> Handle(GetBreedByIdCommand request, CancellationToken cancellationToken)
    {
        var breed = await this.specieService.GetBreedByIdAsync(request.BreedId, cancellationToken);
        return this.mapper.Map<BreedWithSpecieDto>(breed);
    }
}
