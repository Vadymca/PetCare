namespace PetCare.Application.Dtos;
using System;

/// <summary>
/// Data transfer object representing a user after registration.
/// </summary>
public sealed record UserDto(
    Guid id,
    string email,
    string firstName,
    string lastName,
    string phoneNumber,
    string role);
