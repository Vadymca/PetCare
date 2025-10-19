namespace PetCare.Application.Features.Breeds.UpdateBreed;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to update an existing breed and returns the updated breed along with its associated species
/// information.
/// </summary>
/// <remarks>This handler delegates the update operation to the provided species service. It is typically used in
/// a CQRS pattern to process update commands for breed entities. The handler is thread-safe and intended for use in
/// dependency injection scenarios.</remarks>
public sealed class UpdateBreedCommandHandler : IRequestHandler<UpdateBreedCommand, BreedWithSpecieDto>
{
    private readonly ISpecieService specieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBreedCommandHandler"/> class with the specified specie service.
    /// </summary>
    /// <param name="specieService">The service used to retrieve and manage specie information required for breed updates. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="specieService"/> is null.</exception>
    public UpdateBreedCommandHandler(ISpecieService specieService)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
    }

    /// <summary>
    /// Handles the update of a breed by applying the specified changes and returns the updated breed along with its
    /// associated species information.
    /// </summary>
    /// <param name="request">An <see cref="UpdateBreedCommand"/> containing the breed identifier and the updated name and description to
    /// apply.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BreedWithSpecieDto"/> representing the updated breed and its associated species details.</returns>
    public async Task<BreedWithSpecieDto> Handle(UpdateBreedCommand request, CancellationToken cancellationToken)
    {
        return await this.specieService.UpdateBreedAsync(request.Id, request.Name, request.Description, cancellationToken);
    }
}
