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
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly ILogger<GetUserSubscriptionsCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserSubscriptionsCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">User repository for accessing user data.</param>
    /// <param name="mapper">Mapper for converting domain entities to DTOs.</param>
    /// <param name="logger">Logger instance for structured logging.</param>
    /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
    public GetUserSubscriptionsCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserSubscriptionsCommandHandler> logger)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
        var shelters = await this.userRepository.GetUsersByShelterSubscriptionAsync(request.UserId, cancellationToken);
        var animals = await this.userRepository.GetUserAnimalSubscriptionsAsync(request.UserId, cancellationToken);

        // Map to DTOs in application layer (AutoMapper or manual projection)
        var shelterDtos = shelters.Select(s => this.mapper.Map<ShelterDto>(s)).ToList();
        var animalDtos = animals.Select(a => this.mapper.Map<AnimalListDto>(a.Animal)).ToList();

        this.logger.LogInformation(
            "Fetched subscriptions for user {UserId}: {ShelterCount} shelters, {AnimalCount} animals",
            request.UserId,
            shelterDtos.Count,
            animalDtos.Count);

        return new GetUserSubscriptionsResponseDto(shelterDtos, animalDtos);
    }
}
