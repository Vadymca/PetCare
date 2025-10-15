namespace PetCare.Application.Features.Users.Roles;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="AddUserRoleCommand"/> command.
/// Adds a role to a user. All failures are reported via exceptions, which are handled by <see cref="ExceptionHandlingMiddleware"/>.
/// </summary>
public sealed class AddUserRoleCommandHandler : IRequestHandler<AddUserRoleCommand, AddUserRoleResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<AddUserRoleCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserRoleCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service to manage users.</param>
    /// <param name="logger">Logger for tracking operations.</param>
    public AddUserRoleCommandHandler(IUserService userService, ILogger<AddUserRoleCommandHandler> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    /// <summary>
    /// Handles the command to add a role to a user.
    /// Throws exceptions for invalid states.
    /// </summary>
    /// <param name="request">The command containing userId and role to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A DTO indicating success.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the user already has the role.</exception>
    public async Task<AddUserRoleResponseDto> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        // Шукаємо користувача
        var user = await this.userService.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException("Користувач не знайдений.");
        }

        // Додаємо роль
        await this.userService.ReplaceRoleAsync(user, request.Role);
        this.logger.LogInformation("Role {Role} added to user {UserId}", request.Role, request.UserId);

        // Повертаємо DTO лише для успішного результату
        return new AddUserRoleResponseDto(
            Success: true,
            Message: "Роль успішно додана.");
    }
}
