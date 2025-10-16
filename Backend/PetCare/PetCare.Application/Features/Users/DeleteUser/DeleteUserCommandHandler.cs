namespace PetCare.Application.Features.Users.DeleteUser;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="DeleteUserCommand"/> — allows an admin to delete an existing user.
/// Returns a <see cref="DeleteUserResponseDto"/> with a success message.
/// </summary>
public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<DeleteUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service for managing user operations.</param>
    /// <param name="logger">Logger instance for recording actions and events.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    public DeleteUserCommandHandler(IUserService userService, ILogger<DeleteUserCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the deletion of a user by admin.
    /// </summary>
    /// <param name="request">The <see cref="DeleteUserCommand"/> containing the ID of the user to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the operation.</param>
    /// <returns>A <see cref="DeleteUserResponseDto"/> indicating success and providing a message.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the user with the specified ID does not exist.</exception>
    public async Task<DeleteUserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await this.userService.DeleteUserAsync(request.Id, cancellationToken);

        this.logger.LogInformation("User {UserId} has been deleted by admin", request.Id);

        return new DeleteUserResponseDto(
            Success: true,
            Message: $"Користувача {request.Id} успішно видалено.");
    }
}
