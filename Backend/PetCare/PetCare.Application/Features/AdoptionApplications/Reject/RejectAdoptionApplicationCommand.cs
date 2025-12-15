namespace PetCare.Application.Features.AdoptionApplications.Reject;

using System;
using MediatR;

/// <summary>
/// Command to reject an adoption application.
/// </summary>
/// <param name="Id">The ID of the adoption application to reject.</param>
/// <param name="Reason">The reason for rejection.</param>
public sealed record RejectAdoptionApplicationCommand(Guid Id, string Reason) : IRequest<Unit>;
