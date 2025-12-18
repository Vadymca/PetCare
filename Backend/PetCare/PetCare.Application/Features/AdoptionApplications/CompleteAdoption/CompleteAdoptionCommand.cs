namespace PetCare.Application.Features.AdoptionApplications.CompleteAdoption;

using System;
using MediatR;

/// <summary>
/// Command to complete an adoption application.
/// </summary>
public sealed record CompleteAdoptionCommand(
Guid ApplicationId,
bool IsAdopted) : IRequest<Unit>;
