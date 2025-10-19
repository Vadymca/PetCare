namespace PetCare.Application.Features.Breeds.DeleteBreed;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles a command to delete a breed by its identifier and returns the result of the operation.
/// </summary>
/// <remarks>This handler uses the provided ISpecieService to perform the deletion. The operation is asynchronous
/// and supports cancellation via the CancellationToken parameter. Typically used in a CQRS pipeline to process breed
/// deletion requests.</remarks>
public sealed class DeleteBreedCommandHandler : IRequestHandler<DeleteBreedCommand, DeleteBreedResponseDto>
{
    private readonly ISpecieService specieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteBreedCommandHandler"/> class using the specified specie service.
    /// </summary>
    /// <param name="specieService">The service used to manage and retrieve specie-related data required by the command handler. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if the specieService parameter is null.</exception>
    public DeleteBreedCommandHandler(ISpecieService specieService)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
    }

    /// <summary>
    /// Handles a request to delete a breed and returns the result of the operation.
    /// </summary>
    /// <param name="request">The command containing the identifier of the breed to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A response object indicating whether the breed was successfully deleted.</returns>
    public async Task<DeleteBreedResponseDto> Handle(DeleteBreedCommand request, CancellationToken cancellationToken)
    {
        await this.specieService.DeleteBreedAsync(request.Id, cancellationToken);
        return new DeleteBreedResponseDto(true, $"Порода з Id '{request.Id}' успішно видалена.");
    }
}
