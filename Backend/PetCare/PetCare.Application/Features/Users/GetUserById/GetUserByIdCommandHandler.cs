namespace PetCare.Application.Features.Users.GetUserById;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="GetUserByIdCommand"/> to retrieve a specific user by Id.
/// </summary>
public sealed class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, UserDto>
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service for retrieving user roles.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public GetUserByIdCommandHandler(
        IUserService userService,
        IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Handles the command to retrieve a user by their unique Id.
    /// </summary>
    /// <param name="request">The command containing the user's Id.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The <see cref="UserDto"/> representing the user.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user is not found.</exception>
    public async Task<UserDto> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByIdAsync(request.Id)
       ?? throw new InvalidOperationException("Користувача не знайдено.");

        var roles = await this.userService.GetRolesAsync(user);
        var userRole = roles.FirstOrDefault() ?? "User";

        return this.mapper.Map<UserDto>(user) with { Role = userRole };
    }
}
