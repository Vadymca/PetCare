namespace PetCare.Application.Features.Users.UpdateUser;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a command to update the details of an existing user.
/// </summary>
/// <param name="Id">The unique identifier of the user to update.</param>
/// <param name="Email">The new email address for the user, or null to leave unchanged.</param>
/// <param name="Password">The new password for the user, or null to leave unchanged.</param>
/// <param name="FirstName">The new first name for the user, or null to leave unchanged.</param>
/// <param name="LastName">The new last name for the user, or null to leave unchanged.</param>
/// <param name="Phone">The new phone number for the user, or null to leave unchanged.</param>
/// <param name="Preferences">A dictionary of user preferences to update, or null to leave unchanged. Each key-value pair represents a preference
/// name and its value.</param>
/// <param name="Points">The new points value for the user, or null to leave unchanged.</param>
/// <param name="ProfilePhoto">The new profile photo URL or identifier for the user, or null to leave unchanged.</param>
/// <param name="Language">The new preferred language for the user, or null to leave unchanged.</param>
/// <param name="PostalCode">The new postal code for the user, or null to leave unchanged.</param>
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
