namespace PetCare.Application.Features.Animals.GetFavoriteAnimals;

using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Handler for the <see cref="GetFavoriteAnimalsCommand"/>.
/// </summary>
public sealed class GetFavoriteAnimalsCommandHandler : IRequestHandler<GetFavoriteAnimalsCommand, IReadOnlyList<AnimalListDto>>
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFavoriteAnimalsCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetFavoriteAnimalsCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<AnimalListDto>> Handle(GetFavoriteAnimalsCommand request, CancellationToken cancellationToken)
    {
        var subscriptions = await userRepository.GetUserAnimalSubscriptionsAsync(request.UserId, cancellationToken);
        var animals = subscriptions.Select(s => s.Animal!).ToList();
        return mapper.Map<IReadOnlyList<AnimalListDto>>(animals);
    }
}
