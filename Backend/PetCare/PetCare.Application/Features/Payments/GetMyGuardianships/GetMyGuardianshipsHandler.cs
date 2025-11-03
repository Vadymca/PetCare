namespace PetCare.Application.Features.Payments.GetMyGuardianships;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to retrieve the list of guardianships associated with the current user.
/// </summary>
/// <remarks>This handler uses the provided guardianship service to fetch guardianship records for a user and maps
/// them to data transfer objects suitable for client consumption. It is typically used in scenarios where a user needs
/// to view their own guardianship relationships.</remarks>
public sealed class GetMyGuardianshipsHandler : IRequestHandler<GetMyGuardianshipsCommand, IReadOnlyList<MyGuardianshipDto>>
{
    private readonly IGuardianshipService guardianships;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyGuardianshipsHandler"/> class using the specified guardianship service.
    /// </summary>
    /// <param name="guardianships">The service used to retrieve guardianship information. Cannot be null.</param>
    public GetMyGuardianshipsHandler(IGuardianshipService guardianships) => this.guardianships = guardianships;

    /// <summary>
    /// Retrieves a read-only list of guardianship records associated with the specified user.
    /// </summary>
    /// <param name="request">The command containing the user identifier for which guardianship records are to be retrieved.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of guardianship data transfer objects for the specified user. The list will be empty if no
    /// guardianships are found.</returns>
    public async Task<IReadOnlyList<MyGuardianshipDto>> Handle(GetMyGuardianshipsCommand request, CancellationToken ct)
    {
        var items = await this.guardianships.GetByUserAsync(request.UserId, null, ct);
        return items.Select(g => new MyGuardianshipDto(
            g.Id,
            g.AnimalId,
            g.Animal!.Name.Value,
            g.Animal.Slug.Value,
            g.StartDate,
            g.Status.ToString())).ToList();
    }
}
