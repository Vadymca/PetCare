namespace PetCare.Application.Features.AdoptionApplications.ChangeStatus;

using System;
using MediatR;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles <see cref="ChangeAdoptionApplicationStatusCommand"/>.
/// </summary>
public sealed class ChangeAdoptionApplicationStatusCommandHandler
    : IRequestHandler<ChangeAdoptionApplicationStatusCommand>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeAdoptionApplicationStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service responsible for managing adoption applications.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="adoptionApplicationService"/> is <c>null</c>.
    /// </exception>
    public ChangeAdoptionApplicationStatusCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task Handle(
        ChangeAdoptionApplicationStatusCommand request,
        CancellationToken cancellationToken)
    {
        switch (request.Status)
        {
            case AdoptionStatus.Approved:
                await this.adoptionApplicationService.ApproveAsync(
                request.Id,
                request.AdminId,
                request.CuratorName,
                request.CuratorPhone,
                cancellationToken);
                break;

            case AdoptionStatus.Rejected:
                await this.adoptionApplicationService.RejectAsync(
                    request.Id,
                    request.RejectionReason!,
                    cancellationToken);
                break;

            default:
                throw new InvalidOperationException("Недопустима зміна статусу заявки.");
        }
    }
}
