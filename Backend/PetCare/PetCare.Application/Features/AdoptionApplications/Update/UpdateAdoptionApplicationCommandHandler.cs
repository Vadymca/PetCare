namespace PetCare.Application.Features.AdoptionApplications.Update;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="UpdateAdoptionApplicationCommand"/>.
/// </summary>
public sealed class UpdateAdoptionApplicationCommandHandler
    : IRequestHandler<UpdateAdoptionApplicationCommand, AdoptionApplicationDetailsDto>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAdoptionApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service responsible for managing adoption applications.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="adoptionApplicationService"/> is <c>null</c>.
    /// </exception>
    public UpdateAdoptionApplicationCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplicationDetailsDto> Handle(
        UpdateAdoptionApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = await this.adoptionApplicationService.UpdateAsync(
            request.Id,
            request.Comment,
            request.AdminNotes,
            cancellationToken);

        return new AdoptionApplicationDetailsDto(
            application.Id,
            application.UserId,
            application.AnimalId,
            application.Status,
            application.ApplicationDate,
            application.MeetingDate,
            application.AdoptionDate,
            application.RejectionDate,
            application.Comment ?? string.Empty,
            application.AdminNotes ?? string.Empty,
            application.RejectionReason ?? string.Empty,
            application.CuratorName,
            application.CuratorPhone,
            application.CreatedAt,
            application.UpdatedAt);
    }
}
