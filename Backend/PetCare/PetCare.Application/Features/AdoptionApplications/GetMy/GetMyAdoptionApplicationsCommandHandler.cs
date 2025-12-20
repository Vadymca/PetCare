namespace PetCare.Application.Features.AdoptionApplications.GetMy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles <see cref="GetMyAdoptionApplicationsCommand"/>.
/// </summary>
public sealed class GetMyAdoptionApplicationsCommandHandler
    : IRequestHandler<GetMyAdoptionApplicationsCommand, IReadOnlyList<AdoptionApplicationDetailsDto>>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyAdoptionApplicationsCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service responsible for managing adoption applications.
    /// </param>
    public GetMyAdoptionApplicationsCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplicationDetailsDto>> Handle(
        GetMyAdoptionApplicationsCommand request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(request.UserId));
        }

        var applications = await this.adoptionApplicationService.GetByUserAsync(
            request.UserId,
            cancellationToken);

        return applications
            .Select(a =>
            {
                DateTime? adoptionDate = null;
                DateTime? rejectionDate = null;

                switch (a.Status)
                {
                    case AdoptionStatus.Rejected:
                        rejectionDate = a.RejectionDate;
                        break;

                    case AdoptionStatus.Completed: // усиновлено
                        adoptionDate = a.AdoptionDate;
                        break;

                        // Pending та Approved — обидві дати null
                }

                return new AdoptionApplicationDetailsDto(
                    a.Id,
                    a.UserId,
                    a.AnimalId,
                    a.Status,
                    a.ApplicationDate,
                    a.MeetingDate,
                    adoptionDate,
                    rejectionDate,
                    a.Comment ?? string.Empty,
                    a.AdminNotes ?? string.Empty,
                    a.RejectionReason ?? string.Empty,
                    a.CuratorName,
                    a.CuratorPhone,
                    a.CreatedAt,
                    a.UpdatedAt);
            })
            .OrderByDescending(a => a.CreatedAt) // найсвіжіші зверху
            .ToList();
    }
}
