namespace PetCare.Application.Features.Payments.GetMySubscriptions;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>Get current user's recurring subscriptions.</summary>
public sealed record GetMySubscriptionsCommand(Guid UserId) : IRequest<IReadOnlyList<MySubscriptionDto>>;
