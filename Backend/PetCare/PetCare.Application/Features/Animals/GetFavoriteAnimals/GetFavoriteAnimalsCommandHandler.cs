namespace PetCare.Application.Features.Animals.GetFavoriteAnimals;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for the <see cref="GetFavoriteAnimalsCommand"/>.
/// </summary>
public sealed class GetFavoriteAnimalsCommandHandler : IRequestHandler<GetFavoriteAnimalsCommand, IReadOnlyList<AnimalListDto>>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFavoriteAnimalsCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetFavoriteAnimalsCommandHandler(IUserService userService, IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AnimalListDto>> Handle(GetFavoriteAnimalsCommand request, CancellationToken cancellationToken)
    {
        var subscriptions = await this.userService.GetUserAnimalSubscriptionsAsync(request.UserId, cancellationToken);
        var animals = subscriptions.Select(s => s.Animal!).ToList();
        return this.mapper.Map<IReadOnlyList<AnimalListDto>>(animals);
    }
}
