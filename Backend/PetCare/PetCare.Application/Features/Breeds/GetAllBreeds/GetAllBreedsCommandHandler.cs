namespace PetCare.Application.Features.Breeds.GetAllBreeds;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve all available breeds by delegating to the species service.
/// </summary>
/// <remarks>This handler is typically used in a CQRS pattern to process commands for fetching breed data. It
/// relies on an implementation of <see cref="ISpecieService"/> to perform the retrieval operation. The handler is
/// thread-safe and intended for use in dependency injection scenarios.</remarks>
public sealed class GetAllBreedsCommandHandler : IRequestHandler<GetAllBreedsCommand, GetAllBreedsResponseDto>
{
    private readonly ISpecieService specieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllBreedsCommandHandler"/> class using the specified specie service.
    /// </summary>
    /// <param name="specieService">The service used to retrieve specie information required by the command handler. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specieService"/> is null.</exception>
    public GetAllBreedsCommandHandler(ISpecieService specieService)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
    }

    /// <summary>
    /// Handles the specified command to retrieve all available breeds.
    /// </summary>
    /// <param name="request">The command containing any parameters required to retrieve the list of breeds.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A response object containing the collection of all breeds.</returns>
    public async Task<GetAllBreedsResponseDto> Handle(GetAllBreedsCommand request, CancellationToken cancellationToken)
    {
        return await this.specieService.GetAllBreedsAsync(cancellationToken);
    }
}
