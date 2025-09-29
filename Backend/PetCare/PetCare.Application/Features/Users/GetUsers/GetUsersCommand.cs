namespace PetCare.Application.Features.Users.GetUsers;

using MediatR;
using PetCare.Application.Dtos.UserDtos;

public sealed record GetUsersCommand(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    string? Role = null
) : IRequest<GetUsersResponseDto>;
