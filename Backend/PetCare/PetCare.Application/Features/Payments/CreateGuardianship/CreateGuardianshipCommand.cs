namespace PetCare.Application.Features.Payments.CreateGuardianship;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Command to create a new guardianship for a specified user and animal.
/// </summary>
/// <param name="UserId">The unique identifier of the user creating the guardianship.</param>
/// <param name="AnimalId">The unique identifier of the animal to be placed under guardianship.</param>
public sealed record CreateGuardianshipCommand(Guid UserId, Guid AnimalId)
    : IRequest<GuardianshipCreatedDto>;
