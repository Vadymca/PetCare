namespace PetCare.Application.Features.Shelters.UnsubscribeFromShelter;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the command to unsubscribe a user from a shelter.
/// </summary>
/// <remarks>This handler processes requests to remove a user's subscription from a specified shelter. It relies
/// on the provided shelter service to perform the unsubscribe operation. The handler returns a result indicating the
/// outcome of the unsubscribe action. This class is typically used in a CQRS or MediatR pipeline to encapsulate the
/// unsubscribe logic.</remarks>
public class UnsubscribeFromShelterCommandHandler : IRequestHandler<UnsubscribeFromShelterCommand, UnsubscribeResultDto>
{
    private readonly IShelterService shelterService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeFromShelterCommandHandler"/> class with the specified shelter service.
    /// </summary>
    /// <param name="shelterService">The shelter service used to process unsubscribe commands. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="shelterService"/> is null.</exception>
    public UnsubscribeFromShelterCommandHandler(IShelterService shelterService)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
    }

    /// <summary>
    /// Processes a request to unsubscribe a user from a shelter and returns the result of the operation.
    /// </summary>
    /// <param name="request">An object containing the details of the unsubscribe request, including the shelter and user identifiers.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A <see cref="UnsubscribeResultDto"/> containing the outcome of the unsubscribe request, including a confirmation
    /// message.</returns>
    public async Task<UnsubscribeResultDto> Handle(UnsubscribeFromShelterCommand request, CancellationToken cancellationToken)
    {
        await this.shelterService.UnsubscribeUserAsync(request.ShelterId, request.UserId, cancellationToken);
        return new UnsubscribeResultDto("Ви успішно відписані від притулку.");
    }
}
