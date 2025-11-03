namespace PetCare.Application.Features.Payments.CreateGuardianship;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the creation of new guardianships.
/// </summary>
public sealed class CreateGuardianshipCommandHandler
    : IRequestHandler<CreateGuardianshipCommand, GuardianshipCreatedDto>
{
    private readonly IGuardianshipService guardianshipService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGuardianshipCommandHandler"/> class with the specified guardianship.
    /// service.
    /// </summary>
    /// <param name="guardianshipService">The service used to perform guardianship-related operations. Cannot be null.</param>
    public CreateGuardianshipCommandHandler(IGuardianshipService guardianshipService)
    {
        this.guardianshipService = guardianshipService;
    }

    /// <inheritdoc/>
    public async Task<GuardianshipCreatedDto> Handle(CreateGuardianshipCommand request, CancellationToken cancellationToken)
    {
        var guardianship = await this.guardianshipService.CreateAsync(request.UserId, request.AnimalId, cancellationToken: cancellationToken);

        return new GuardianshipCreatedDto(
            guardianship.Id,
            guardianship.AnimalId,
            guardianship.StartDate,
            guardianship.GraceUntil ?? DateTime.UtcNow.AddDays(3),
            guardianship.Status.ToString());
    }
}
