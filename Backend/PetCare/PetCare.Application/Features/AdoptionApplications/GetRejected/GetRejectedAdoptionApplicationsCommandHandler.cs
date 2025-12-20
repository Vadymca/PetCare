namespace PetCare.Application.Features.AdoptionApplications.GetRejected;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles <see cref="GetRejectedAdoptionApplicationsCommand"/>.
/// </summary>
public sealed class GetRejectedAdoptionApplicationsCommandHandler
    : IRequestHandler<GetRejectedAdoptionApplicationsCommand, IReadOnlyList<AdoptionApplicationDetailsDto>>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRejectedAdoptionApplicationsCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">The service responsible for managing adoption applications.</param>
    public GetRejectedAdoptionApplicationsCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplicationDetailsDto>> Handle(
        GetRejectedAdoptionApplicationsCommand request,
        CancellationToken cancellationToken)
    {
        var applications = await this.adoptionApplicationService.GetByStatusAsync(
            AdoptionStatus.Rejected, cancellationToken);

        return applications
            .Select(a => new AdoptionApplicationDetailsDto(
                a.Id,
                a.UserId,
                a.AnimalId,
                a.Status,
                a.ApplicationDate,
                a.MeetingDate,
                a.AdoptionDate,
                a.RejectionDate,
                a.Comment ?? string.Empty,
                a.AdminNotes ?? string.Empty,
                a.RejectionReason ?? string.Empty,
                a.CuratorName,
                a.CuratorPhone,
                a.CreatedAt,
                a.UpdatedAt))
            .OrderByDescending(a => a.CreatedAt) // найсвіжіші зверху
            .ToList();
    }
}
