namespace PetCare.Application.Features.Payments.LiqPay.LiqPayCallback;

using MediatR;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Command triggered when LiqPay sends a payment callback.
/// </summary>
/// <param name="Data">Base64-encoded payment data from LiqPay.</param>
/// <param name="Signature">Base64-encoded signature used for verification.</param>
public sealed record HandleLiqPayCallbackCommand(string Data, string Signature)
    : IRequest<IResult>;
