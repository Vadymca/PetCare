namespace PetCare.Application.Features.PaymentMethods.GetAllPaymentMethods;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve all available payment methods.
/// </summary>
/// <remarks>Use this command to obtain a read-only list of payment methods currently supported by the system. The
/// returned list may be empty if no payment methods are configured.</remarks>
public sealed record GetAllPaymentMethodsCommand() : IRequest<IReadOnlyList<PaymentMethodDto>>;
