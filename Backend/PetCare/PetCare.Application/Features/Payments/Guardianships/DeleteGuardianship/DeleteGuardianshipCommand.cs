namespace PetCare.Application.Features.Payments.Guardianships.DeleteGuardianship;

using System;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Command to delete a guardianship by its ID.
/// </summary>
/// <param name="GuardianshipId">The unique identifier of the guardianship to delete.</param>
/// <param name="UserId">ID of the user requesting the deletion (ownership validation may be applied).</param>
public sealed record DeleteGuardianshipCommand(Guid GuardianshipId, Guid UserId)
    : IRequest<GuardianshipDeletedDto>;
