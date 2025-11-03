namespace PetCare.Application.Features.Payments.GetMyUpcomingPayments;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>Get current user's upcoming expected payments.</summary>
public sealed record GetMyUpcomingPaymentsCommand(Guid UserId) : IRequest<IReadOnlyList<MyUpcomingPaymentDto>>;
