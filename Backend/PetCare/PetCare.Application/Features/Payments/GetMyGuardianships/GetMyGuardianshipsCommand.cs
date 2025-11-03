namespace PetCare.Application.Features.Payments.GetMyGuardianships;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>Get current user's guardianships.</summary>
public sealed record GetMyGuardianshipsCommand(Guid UserId) : IRequest<IReadOnlyList<MyGuardianshipDto>>;
