namespace PetCare.Application.Features.Users.GetUsers;

using MediatR;
using PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents a request to retrieve a paginated list of users, optionally filtered by search criteria and role.
/// </summary>
/// <param name="Page">The page number of results to retrieve. Must be greater than or equal to 1.</param>
/// <param name="PageSize">The maximum number of users to include in a single page of results. Must be greater than 0.</param>
/// <param name="Search">An optional search term to filter users by name, email, or other relevant fields. If null or empty, no search
/// filtering is applied.</param>
/// <param name="Role">An optional role name to filter users by their assigned role. If null or empty, users of all roles are included.</param>
public sealed record GetUsersCommand(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    string? Role = null
) : IRequest<GetUsersResponseDto>;
