namespace PetCare.Application.Features.AnimalAidRequests.Delete;

using System;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles deletion of an <see cref="PetCare.Domain.Entities.AnimalAidRequest"/>.
/// </summary>
public sealed class DeleteAnimalAidRequestCommandHandler
    : IRequestHandler<DeleteAnimalAidRequestCommand, Unit>
{
    private readonly IAnimalAidRequestService animalAidRequestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAnimalAidRequestCommandHandler"/> class.
    /// </summary>
    /// <param name="animalAidRequestService">
    /// The service responsible for managing <see cref="PetCare.Domain.Entities.AnimalAidRequest"/> operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="animalAidRequestService"/> is <c>null</c>.
    /// </exception>
    public DeleteAnimalAidRequestCommandHandler(
        IAnimalAidRequestService animalAidRequestService)
    {
        this.animalAidRequestService = animalAidRequestService
            ?? throw new ArgumentNullException(nameof(animalAidRequestService));
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(
        DeleteAnimalAidRequestCommand request,
        CancellationToken cancellationToken)
    {
        await this.animalAidRequestService.DeleteAnimalAidRequestAsync(
            request.Id,
            cancellationToken).ConfigureAwait(false);

        return Unit.Value;
    }
}
