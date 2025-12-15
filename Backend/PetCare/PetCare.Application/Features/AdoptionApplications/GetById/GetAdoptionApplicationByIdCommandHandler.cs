namespace PetCare.Application.Features.AdoptionApplications.GetById;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAdoptionApplicationByIdCommand"/>.
/// </summary>
public sealed class GetAdoptionApplicationByIdCommandHandler
    : IRequestHandler<GetAdoptionApplicationByIdCommand, AdoptionApplicationDetailsDto>
{
    private readonly IAdoptionApplicationService adoptionApplicationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAdoptionApplicationByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionApplicationService">
    /// The service used to retrieve adoption applications.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="adoptionApplicationService"/> is <c>null</c>.
    /// </exception>
    public GetAdoptionApplicationByIdCommandHandler(
        IAdoptionApplicationService adoptionApplicationService)
    {
        this.adoptionApplicationService = adoptionApplicationService
            ?? throw new ArgumentNullException(nameof(adoptionApplicationService));
    }

    /// <inheritdoc/>
    public async Task<AdoptionApplicationDetailsDto> Handle(
        GetAdoptionApplicationByIdCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор заявки не може бути порожнім.", nameof(request.Id));
        }

        var application = await this.adoptionApplicationService
            .GetByIdAsync(request.Id, cancellationToken);

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
