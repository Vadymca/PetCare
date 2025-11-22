namespace PetCare.Application.Features.Payments.Guardianships.GetMyGuardianships;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
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
    private readonly ISubscriptionService subscriptions;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyGuardianshipsHandler"/> class using the specified guardianship service.
    /// </summary>
    /// <param name="guardianships">The service used to retrieve guardianship information. Cannot be null.</param>
    /// <param name="subscriptions">The service used to manage subscriptions. Cannot be null.</param>
    /// <param name="mapper">The mapper used for object-object mapping. Cannot be null.</param>
    public GetMyGuardianshipsHandler(
         IGuardianshipService guardianships,
         ISubscriptionService subscriptions,
         IMapper mapper)
    {
        this.guardianships = guardianships ?? throw new ArgumentNullException(nameof(guardianships));
        this.subscriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Retrieves a read-only list of guardianship records associated with the specified user.
    /// </summary>
    /// <param name="request">The command containing the user identifier for which guardianship records are to be retrieved.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of guardianship data transfer objects for the specified user. The list will be empty if no
    /// guardianships are found.</returns>
    public async Task<IReadOnlyList<MyGuardianshipDto>> Handle(GetMyGuardianshipsCommand request, CancellationToken ct)
    {
        var list = await this.guardianships.GetByUserAsync(request.UserId, null, ct);

        var result = new List<MyGuardianshipDto>();

        foreach (var g in list)
        {
            if (g.Animal is null)
            {
                continue;
            }

            var animalDto = this.mapper.Map<AnimalDto>(g.Animal);

            var sub = await this.subscriptions.GetByGuardianshipIdAsync(g.Id, ct);

            PaymentSubscriptionDto? subDto = null;

            if (sub is not null)
            {
                var isOverdue = sub.NextChargeAt < DateTime.UtcNow;

                subDto = new PaymentSubscriptionDto(
                    sub.Id,
                    sub.Amount,
                    sub.Currency,
                    sub.NextChargeAt,
                    sub.Status.ToString(),
                    isOverdue);
            }

            result.Add(new MyGuardianshipDto(
                g.Id,
                g.StartDate,
                g.GraceUntil ?? DateTime.UtcNow.AddDays(3),
                g.Status.ToString(),
                animalDto,
                subDto));
        }

        return result;
    }
}
