namespace PetCare.Application.Features.Users.GetCurrentUser;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetCurrentUserCommand"/> — returns the profile of the current user.
/// </summary>
public sealed class GetCurrentUserCommandHandler : IRequestHandler<GetCurrentUserCommand, UserDto>
{
    private readonly IUserRepository userRepository;
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly ILogger<GetCurrentUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCurrentUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Repository for accessing user data.</param>
    /// <param name="userService">Service to retrieve user roles.</param>
    /// <param name="mapper">AutoMapper instance for mapping entities to DTOs.</param>
    /// <param name="logger">Logger instance.</param>
    public GetCurrentUserCommandHandler(
        IUserRepository userRepository,
        IUserService userService,
        IMapper mapper,
        ILogger<GetCurrentUserCommandHandler> logger)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="GetCurrentUserCommand"/> to return the current user's profile.
    /// </summary>
    /// <param name="request">The command containing the user's ID (extracted from token).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The <see cref="UserDto"/> representing the current user.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    public async Task<UserDto> Handle(GetCurrentUserCommand request, CancellationToken cancellationToken)
    {
        // Retrieve user by ID
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Користувача не знайдено.");

        // Map user entity to DTO
        var userDto = this.mapper.Map<UserDto>(user);

        // Get roles and assign the first role or default to "User"
        var roles = await this.userService.GetRolesAsync(user);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        this.logger.LogInformation("Fetched profile for user {UserId}", request.UserId);

        return userDto;
    }
}
