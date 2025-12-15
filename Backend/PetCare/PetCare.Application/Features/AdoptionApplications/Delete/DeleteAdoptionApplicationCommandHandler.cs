namespace PetCare.Application.Features.AdoptionApplications.Delete;

using System;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="DeleteAdoptionApplicationCommand"/>.
/// </summary>
public sealed class DeleteAdoptionApplicationCommandHandler
    : IRequestHandler<DeleteAdoptionApplicationCommand>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAdoptionApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service responsible for managing adoption applications.
    /// </param>
    public DeleteAdoptionApplicationCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task Handle(
        DeleteAdoptionApplicationCommand request,
        CancellationToken cancellationToken)
    {
        await this.adoptionApplicationService.DeleteAsync(
            request.Id,
            cancellationToken);
    }
}
