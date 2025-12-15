namespace PetCare.Application.Features.AdoptionApplications.Approve;

using System;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles approving an adoption application.
/// </summary>
public sealed class ApproveAdoptionApplicationCommandHandler
 : IRequestHandler<ApproveAdoptionApplicationCommand, Unit>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApproveAdoptionApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">The service responsible for adoption application operations.</param>
    public ApproveAdoptionApplicationCommandHandler(IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(ApproveAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Id заявки не може бути порожнім.", nameof(request.Id));
        }

        if (request.AdminId == Guid.Empty)
        {
            throw new ArgumentException("Id адміністратора не може бути порожнім.", nameof(request.AdminId));
        }

        await this.adoptionApplicationService.ApproveAsync(
            request.Id,
            request.AdminId,
            request.CuratorName,
            request.CuratorPhone,
            cancellationToken);

        return Unit.Value;
    }
}
