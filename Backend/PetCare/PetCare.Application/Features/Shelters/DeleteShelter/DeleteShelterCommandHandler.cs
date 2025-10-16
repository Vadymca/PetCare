namespace PetCare.Application.Features.Shelters.DeleteShelter;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the command to delete a shelter by delegating the operation to the shelter service.
/// </summary>
/// <remarks>This handler is typically used in a CQRS pipeline to process requests for removing shelters from the
/// system. It returns a response indicating whether the deletion was successful and includes a message describing the
/// outcome.</remarks>
public sealed class DeleteShelterCommandHandler : IRequestHandler<DeleteShelterCommand, DeleteShelterResponseDto>
{
    private readonly IShelterService shelterService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteShelterCommandHandler"/> class using the specified shelter service.
    /// </summary>
    /// <param name="shelterService">The service used to perform shelter-related operations. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shelterService"/> is null.</exception>
    public DeleteShelterCommandHandler(IShelterService shelterService)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
    }

    /// <summary>
    /// Handles a request to delete a shelter and returns the result of the operation.
    /// </summary>
    /// <param name="request">The command containing the identifier of the shelter to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A response object indicating whether the shelter was successfully deleted, along with a descriptive message.</returns>
    public async Task<DeleteShelterResponseDto> Handle(DeleteShelterCommand request, CancellationToken cancellationToken)
    {
        await this.shelterService.DeleteAsync(request.Id, cancellationToken);
        return new DeleteShelterResponseDto(true, $"Притулок з Id '{request.Id}' успішно видалено.");
    }
}
