namespace PetCare.Application.Features.AdoptionApplications.Delete;

using System;
using MediatR;

/// <summary>
/// Command to delete an adoption application.
/// </summary>
/// <param name="Id">The identifier of the adoption application.</param>
public sealed record DeleteAdoptionApplicationCommand(Guid Id) : IRequest;
