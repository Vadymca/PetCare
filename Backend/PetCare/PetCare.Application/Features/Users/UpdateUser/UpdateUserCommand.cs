namespace PetCare.Application.Features.Users.UpdateUser;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using System;
using System.Collections.Generic;

public sealed record UpdateUserCommand(
Guid Id,
string? Email,
string? Password,
string? FirstName,
string? LastName,
string? Phone,
Dictionary<string, string>? Preferences,
int? Points,
string? ProfilePhoto,
string? Language,
string? PostalCode) : IRequest<UserDto>;
