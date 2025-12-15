namespace PetCare.Application.Features.AdoptionApplications.GetApproved;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles <see cref="GetApprovedAdoptionApplicationsCommand"/>.
/// </summary>
public sealed class GetApprovedAdoptionApplicationsCommandHandler
    : IRequestHandler<GetApprovedAdoptionApplicationsCommand, IReadOnlyList<AdoptionApplicationListDto>>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetApprovedAdoptionApplicationsCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service responsible for managing adoption applications.
    /// </param>
    public GetApprovedAdoptionApplicationsCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplicationListDto>> Handle(
        GetApprovedAdoptionApplicationsCommand request,
        CancellationToken cancellationToken)
    {
        var applications = await this.adoptionApplicationService.GetByStatusAsync(
            AdoptionStatus.Approved, cancellationToken);

        return applications
            .Select(a => new AdoptionApplicationListDto(
                a.Id,
                a.UserId,
                a.AnimalId,
                a.Status,
                a.ApplicationDate,
                a.Comment ?? string.Empty,
                a.AdminNotes ?? string.Empty))
            .ToList();
    }
}
