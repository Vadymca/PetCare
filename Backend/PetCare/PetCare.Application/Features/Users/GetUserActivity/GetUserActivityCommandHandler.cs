namespace PetCare.Application.Features.Users.GetUserActivity;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Dtos.EventDtos;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Returns user's activity including adoption applications and event participation mapped to DTOs.
/// </summary>
public sealed class GetUserActivityCommandHandler
: IRequestHandler<GetUserActivityCommand, GetUserActivityResponseDto>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly ILogger<GetUserActivityCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserActivityCommandHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger instance for structured logging.</param>
    /// <param name="mapper">Mapper for converting domain entities to DTOs.</param>
    /// <param name="userService">Service for user-related operations.</param>
    public GetUserActivityCommandHandler(
        IUserService userService,
        IMapper mapper,
        ILogger<GetUserActivityCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="GetUserActivityCommand"/> request by fetching
    /// the user's adoption applications and events, mapping them to DTOs, and returning them in a response DTO.
    /// </summary>
    /// <param name="request">The command containing the user identifier.</param>
    /// <param name="cancellationToken">Token for cancellation.</param>
    /// <returns>A <see cref="GetUserActivityResponseDto"/> containing the user's adoption applications and events.</returns>
    public async Task<GetUserActivityResponseDto> Handle(
        GetUserActivityCommand request,
        CancellationToken cancellationToken)
    {
        var applications = await this.userService.GetUserAdoptionApplicationsAsync(request.UserId, cancellationToken);
        var events = await this.userService.GetUserEventsAsync(request.UserId, cancellationToken);

        var applicationDtos = applications.Select(a => this.mapper.Map<AdoptionApplicationDto>(a)).ToList();
        var eventDtos = events.Select(e => this.mapper.Map<EventDto>(e)).ToList();

        this.logger.LogInformation(
            "Fetched activity for user {UserId}: {ApplicationsCount} applications, {EventsCount} events",
            request.UserId,
            applicationDtos.Count,
            eventDtos.Count);

        return new GetUserActivityResponseDto(applicationDtos, eventDtos);
    }
}
