namespace PetCare.Application.Features.Species.GetSpecieById;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve detailed information about a species by its unique identifier.
/// </summary>
/// <remarks>This handler uses the provided ISpecieService to fetch species data and maps the result to a
/// SpecieDetailDto. It is typically used in scenarios where detailed species information is required, such as
/// displaying species profiles in an application. The handler is thread-safe and intended for use within a MediatR
/// pipeline.</remarks>
public sealed class GetSpecieByIdCommandHandler : IRequestHandler<GetSpecieByIdCommand, SpecieDetailDto>
{
    private readonly ISpecieService specieService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSpecieByIdCommandHandler"/> class with the specified specie service and object.
    /// mapper.
    /// </summary>
    /// <param name="specieService">The service used to retrieve specie data.</param>
    /// <param name="mapper">The mapper used to convert specie entities to data transfer objects.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specieService"/> or <paramref name="mapper"/> is <see langword="null"/>.</exception>
    public GetSpecieByIdCommandHandler(ISpecieService specieService, IMapper mapper)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves detailed information about a species based on the specified request.
    /// </summary>
    /// <param name="request">The command containing the identifier of the species to retrieve. Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="SpecieDetailDto"/> with
    /// the details of the requested species.</returns>
    public async Task<SpecieDetailDto> Handle(GetSpecieByIdCommand request, CancellationToken cancellationToken)
    {
        var specie = await this.specieService.GetSpeciesByIdAsync(request.Id, cancellationToken);
        return this.mapper.Map<SpecieDetailDto>(specie);
    }
}
