namespace PetCare.Application.Features.Users.GetUserSubscriptions;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Returns user's subscribed shelters and animals mapped to DTOs.
/// </summary>
public sealed class GetUserSubscriptionsCommandHandler
    : IRequestHandler<GetUserSubscriptionsCommand, GetUserSubscriptionsResponseDto>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly ILogger<GetUserSubscriptionsCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserSubscriptionsCommandHandler"/> class with the specified user service,.
    /// mapper, and logger.
    /// </summary>
    /// <param name="userService">The service used to retrieve user information and subscriptions.</param>
    /// <param name="mapper">The mapper used to convert user and subscription data between domain and DTO representations.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for this handler.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    public GetUserSubscriptionsCommandHandler(
        IUserService userService,
        IMapper mapper,
        ILogger<GetUserSubscriptionsCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="GetUserSubscriptionsCommand"/> request by fetching
    /// user's subscribed shelters and animals, mapping them to DTOs, and returning the result.
    /// </summary>
    /// <param name="request">The command containing the user identifier.</param>
    /// <param name="cancellationToken">Token for cancellation.</param>
    /// <returns>A <see cref="GetUserSubscriptionsResponseDto"/> containing subscribed shelters and animals.</returns>
    public async Task<GetUserSubscriptionsResponseDto> Handle(
        GetUserSubscriptionsCommand request,
        CancellationToken cancellationToken)
    {
        var shelters = await this.userService.GetUserShelterSubscriptionsAsync(request.UserId, cancellationToken);
        var animals = await this.userService.GetUserAnimalSubscriptionsAsync(request.UserId, cancellationToken);

        // Map to DTOs in application layer (AutoMapper or manual projection)
        var shelterDtos = shelters.Select(s => this.mapper.Map<ShelterListDto>(s.Shelter)).ToList();
        var animalDtos = animals.Select(a => this.mapper.Map<AnimalListDto>(a.Animal)).ToList();

        this.logger.LogInformation(
            "Fetched subscriptions for user {UserId}: {ShelterCount} shelters, {AnimalCount} animals",
            request.UserId,
            shelterDtos.Count,
            animalDtos.Count);

        return new GetUserSubscriptionsResponseDto(shelterDtos, animalDtos);
    }
}
