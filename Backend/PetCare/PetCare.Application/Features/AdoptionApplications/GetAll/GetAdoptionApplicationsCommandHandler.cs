namespace PetCare.Application.Features.AdoptionApplications.GetAll;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for the <see cref="GetAdoptionApplicationsCommand"/>.
/// </summary>
public sealed class GetAdoptionApplicationsCommandHandler
    : IRequestHandler<GetAdoptionApplicationsCommand, IReadOnlyList<AdoptionApplicationDetailsDto>>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAdoptionApplicationsCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">The adoptionAplication instance.</param>
    public GetAdoptionApplicationsCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AdoptionApplicationDetailsDto>> Handle(
        GetAdoptionApplicationsCommand request,
        CancellationToken cancellationToken)
    {
        var applications = await this.adoptionApplicationService
            .GetAllAsync(cancellationToken);

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
            .ToList();
    }
}
