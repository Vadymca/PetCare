namespace PetCare.Application.Features.Payments.GetMyPaymentHistory;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>Get current user's payment history.</summary>
public sealed record GetMyPaymentHistoryCommand(Guid UserId) : IRequest<IReadOnlyList<MyPaymentHistoryDto>>;
