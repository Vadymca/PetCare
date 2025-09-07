namespace PetCare.Application.Dtos.AuthDtos;
using System;

/// <summary>
/// Data transfer object representing a user after registration.
/// </summary>
public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Role,
    string? PostalCode);
