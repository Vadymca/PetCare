namespace PetCare.Application.Features.Users.GetUsers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Dtos.UserDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="GetUsersCommand"/> and returns a paginated list of users with optional filtering.
/// </summary>
public sealed class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, GetUsersResponseDto>
{
    private readonly IMapper mapper;
    private readonly IUserService userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersCommandHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    /// <param name="userService">The user service for retrieving user roles.</param>
    public GetUsersCommandHandler(
        IMapper mapper,
        IUserService userService)
    {
        this.mapper = mapper;
        this.userService = userService;
    }

    /// <summary>
    /// Handles the command to retrieve users with pagination and optional filters.
    /// </summary>
    /// <param name="request">The <see cref="GetUsersCommand"/> containing pagination and filter parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="GetUsersResponseDto"/> containing the list of users and total count.</returns>
    public async Task<GetUsersResponseDto> Handle(GetUsersCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо користувачів із репозиторію з пагінацією та фільтрами
        var (users, totalCount) = await this.userService.GetUsersAsync(
            request.Page,
            request.PageSize,
            request.Search,
            request.Role,
            cancellationToken);

        // Мапимо через AutoMapper
        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await this.userService.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";

            var userDto = this.mapper.Map<UserDto>(user) with
            {
                Role = userRole,
            };

            userDtos.Add(userDto);
        }

        return new GetUsersResponseDto(userDtos, totalCount);
    }
}
