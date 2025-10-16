namespace PetCare.Application.Features.Users.UpdateUser;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles UpdateUserCommand — admin updates an existing user.
/// All business errors are thrown as exceptions (handled by ExceptionHandlingMiddleware).
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly ILogger<UpdateUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class with the specified user service, object mapper,.
    /// and logger.
    /// </summary>
    /// <param name="userService">The service used to perform user-related operations. Cannot be null.</param>
    /// <param name="mapper">The mapper used to convert between domain and data transfer objects. Cannot be null.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for this handler. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="userService"/>, <paramref name="mapper"/>, or <paramref name="logger"/> is null.</exception>
    public UpdateUserCommandHandler(
        IUserService userService,
        IMapper mapper,
        ILogger<UpdateUserCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Викликаємо сервіс, де вся бізнес-логіка (ролі, бали, пароль, адреса, аватар) інкапсульована
        var updatedUser = await this.userService.UpdateUserProfileAsync(
            userId: request.Id,
            firstName: request.FirstName,
            lastName: request.LastName,
            phone: request.Phone,
            profilePhoto: request.ProfilePhoto,
            language: request.Language,
            postalCode: request.PostalCode,
            email: request.Email,
            preferences: request.Preferences,
            points: request.Points,
            password: request.Password,
            cancellationToken: cancellationToken);

        // Мапимо агрегат User у DTO
        var userDto = this.mapper.Map<UserDto>(updatedUser);

        // Додаємо роль користувача (UserService відповідає за ролі)
        var roles = await this.userService.GetRolesAsync(updatedUser);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        this.logger.LogInformation("User {UserId} updated via UserService by admin", request.Id);

        return userDto;
    }
}