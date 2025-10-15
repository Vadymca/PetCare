namespace PetCare.Application.Dtos.UserDtos;

using System.Collections.Generic;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the response returned when retrieving a list of users, including the user data and the total number of
/// users available.
/// </summary>
/// <param name="Users">The collection of user data returned by the request. The list may be empty if no users match the query.</param>
/// <param name="TotalCount">The total number of users that match the query criteria, regardless of paging or filtering applied to the returned
/// list.</param>
public sealed record GetUsersResponseDto(
 IReadOnlyList<UserDto> Users,
 int TotalCount
);
