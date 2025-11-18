namespace PetCare.Application.Features.Payments.Guardianships.CreateGuardianship;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the creation of new guardianships.
/// </summary>
public sealed class CreateGuardianshipCommandHandler
    : IRequestHandler<CreateGuardianshipCommand, GuardianshipCreatedDto>
{
    private readonly IGuardianshipService guardianshipService;
    private readonly ISubscriptionService subscriptionService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGuardianshipCommandHandler"/> class with the specified guardianship.
    /// service.
    /// </summary>
    /// <param name="guardianshipService">The service used to perform guardianship-related operations. Cannot be null.</param>
    /// <param name="subscriptionService">Service for retrieving subscription data.</param>
    /// <param name="mapper">The mapper instance used to convert domain entities to DTOs.</param>
    public CreateGuardianshipCommandHandler(
        IGuardianshipService guardianshipService,
        ISubscriptionService subscriptionService,
        IMapper mapper)
    {
        this.guardianshipService = guardianshipService ?? throw new ArgumentNullException(nameof(guardianshipService));
        this.subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<GuardianshipCreatedDto> Handle(CreateGuardianshipCommand request, CancellationToken cancellationToken)
    {
        var guardianship = await this.guardianshipService
            .CreateAsync(request.UserId, request.AnimalId, cancellationToken: cancellationToken);

        var loaded = await this.guardianshipService
            .GetByIdAsync(guardianship.Id, cancellationToken);

        if (loaded is null || loaded.Animal is null)
        {
            throw new InvalidOperationException("Не вдалося завантажити тварину для створеної опіки.");
        }

        var animalDto = this.mapper.Map<AnimalListDto>(loaded.Animal);

        var subscription = await this.subscriptionService
            .GetByGuardianshipIdAsync(guardianship.Id, cancellationToken);

        PaymentSubscriptionDto? subscriptionDto = null;

        if (subscription is not null)
        {
            var isOverdue = subscription.NextChargeAt < DateTime.UtcNow;

            subscriptionDto = new PaymentSubscriptionDto(
                subscription.Id,
                subscription.Amount,
                subscription.Currency,
                subscription.NextChargeAt,
                subscription.Status.ToString(),
                isOverdue);
        }

        return new GuardianshipCreatedDto(
           guardianship.Id,
           guardianship.AnimalId,
           guardianship.StartDate,
           guardianship.GraceUntil ?? DateTime.UtcNow.AddDays(3),
           guardianship.Status.ToString(),
           animalDto,
           subscriptionDto);
    }
}
