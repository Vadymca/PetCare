namespace PetCare.Application.Features.AdoptionApplications.Reject;

using System;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles rejection of an adoption application.
/// </summary>
public sealed class RejectAdoptionApplicationCommandHandler
    : IRequestHandler<RejectAdoptionApplicationCommand, Unit>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RejectAdoptionApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">Service for managing adoption applications.</param>
    /// <exception cref="ArgumentNullException">Thrown when service is null.</exception>
    public RejectAdoptionApplicationCommandHandler(IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(RejectAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Id заявки не може бути порожнім.", nameof(request.Id));
        }

        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            throw new ArgumentException("Причина відхилення не може бути порожньою.", nameof(request.Reason));
        }

        await this.adoptionApplicationService.RejectAsync(request.Id, request.Reason, cancellationToken);

        return Unit.Value;
    }
}
