namespace PetCare.Application.Features.Payments.Intents;

using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve payment intent details associated with a specific external order identifier.
/// </summary>
/// <param name="ExternalOrderId">The unique identifier of the external order for which to retrieve the payment intent details. Cannot be null or
/// empty.</param>
public sealed record GetPaymentIntentByOrderIdCommand(string ExternalOrderId)
 : IRequest<PaymentIntentDetailsDto?>;
