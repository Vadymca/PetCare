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
    string Phone,
    string Role,
    string? PostalCode,
    string? Address,
    string Language,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int Points,
    string? ProfilePhoto,
    DateTime? LastLogin,
    IReadOnlyDictionary<string, string>? Preferences);
