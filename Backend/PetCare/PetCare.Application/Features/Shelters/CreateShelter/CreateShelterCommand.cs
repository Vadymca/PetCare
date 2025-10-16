namespace PetCare.Application.Features.Shelters.CreateShelter;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a command to create a new shelter.
/// </summary>
/// <param name="Name">The name of the shelter.</param>
/// <param name="Address">The full address of the shelter.</param>
/// <param name="Latitude">Latitude in decimal degrees (-90 to 90).</param>
/// <param name="Longitude">Longitude in decimal degrees (-180 to 180).</param>
/// <param name="ContactPhone">The shelter's contact phone number.</param>
/// <param name="ContactEmail">The shelter's contact email address.</param>
/// <param name="Description">Optional description of the shelter.</param>
/// <param name="Capacity">Maximum capacity of animals the shelter can hold.</param>
/// <param name="CurrentOccupancy">Current number of animals in the shelter.</param>
/// <param name="Photos">Optional list of shelter photos.</param>
/// <param name="VirtualTourUrl">Optional link to a virtual tour.</param>
/// <param name="WorkingHours">Shelter working hours.</param>
/// <param name="SocialMedia">Optional dictionary of social media links.</param>
/// <param name="ManagerId">Optional ID of the assigned manager.</param>
public sealed record CreateShelterCommand(
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    string ContactPhone,
    string ContactEmail,
    string? Description,
    int Capacity,
    List<string>? Photos,
    string? VirtualTourUrl,
    string? WorkingHours,
    Dictionary<string, string>? SocialMedia,
    Guid? ManagerId) : IRequest<ShelterDto>;
