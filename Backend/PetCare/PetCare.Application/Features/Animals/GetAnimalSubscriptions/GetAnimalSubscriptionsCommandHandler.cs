namespace PetCare.Application.Features.Animals.GetAnimalSubscriptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="GetAnimalSubscriptionsCommand"/> request.
/// </summary>
public sealed class GetAnimalSubscriptionsCommandHandler
    : IRequestHandler<GetAnimalSubscriptionsCommand, IReadOnlyList<AnimalListDto>>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalSubscriptionsCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to fetch animal subscriptions.</param>
    /// <param name="mapper">The AutoMapper instance used to map domain entities to DTOs.</param>
    public GetAnimalSubscriptionsCommandHandler(IUserService userService, IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AnimalListDto>> Handle(
        GetAnimalSubscriptionsCommand request,
        CancellationToken cancellationToken)
    {
        // Отримуємо всі підписки користувача
        var subscriptions = await this.userService.GetUserAnimalSubscriptionsAsync(request.UserId, cancellationToken);

        // Беремо самі тварини
        var animals = subscriptions
            .Where(s => s.Animal != null)
            .Select(s => s.Animal!)
            .ToList();

        // Мапимо у DTO
        return this.mapper.Map<IReadOnlyList<AnimalListDto>>(animals);
    }
}
