namespace PetCare.Application.Features.Shelters.GetFavoriteShelters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for the <see cref="GetFavoriteSheltersCommand"/>.
/// </summary>
public sealed class GetFavoriteSheltersCommandHandler : IRequestHandler<GetFavoriteSheltersCommand, IReadOnlyList<ShelterListDto>>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFavoriteSheltersCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetFavoriteSheltersCommandHandler(IUserService userService, IMapper mapper)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ShelterListDto>> Handle(GetFavoriteSheltersCommand request, CancellationToken cancellationToken)
    {
        var subscriptions = await this.userService.GetUserShelterSubscriptionsAsync(request.UserId, cancellationToken);
        var shelters = subscriptions.Select(s => s.Shelter!).ToList();
        return this.mapper.Map<IReadOnlyList<ShelterListDto>>(shelters);
    }
}
