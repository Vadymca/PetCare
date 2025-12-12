namespace PetCare.Application.Features.AnimalAidRequests.Delete;

using System;
using MediatR;

/// <summary>
/// Command to delete an AnimalAidRequest by its identifier.
/// </summary>
/// <param name="Id">The identifier of the request to delete.</param>
public sealed record DeleteAnimalAidRequestCommand(Guid Id) : IRequest<Unit>;