namespace PetCare.Application.Features.Payments.LiqPay.CreateLiqPayCheckout;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles requests to create a LiqPay checkout by processing the command and generating a checkout response.
/// </summary>
/// <remarks>This handler uses an injected ILiqPayClient to build the checkout asynchronously. It validates that
/// the requested amount is greater than zero before proceeding. Typically used within a MediatR pipeline to facilitate
/// payment operations via LiqPay.</remarks>
public sealed class CreateLiqPayCheckoutCommandHandler
 : IRequestHandler<CreateLiqPayCheckoutCommand, LiqPayCheckoutResponseDto>
{
    private readonly ILiqPayClient liqPayClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLiqPayCheckoutCommandHandler"/> class using the specified LiqPay client.
    /// </summary>
    /// <param name="liqPayClient">The client used to interact with the LiqPay payment service. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="liqPayClient"/> is null.</exception>
    public CreateLiqPayCheckoutCommandHandler(ILiqPayClient liqPayClient)
    {
        this.liqPayClient = liqPayClient ?? throw new ArgumentNullException(nameof(liqPayClient));
    }

    /// <inheritdoc/>
    public async Task<LiqPayCheckoutResponseDto> Handle(CreateLiqPayCheckoutCommand request, CancellationToken cancellationToken)
    {
        if (request.Request.Amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        var checkout = await this.liqPayClient.BuildCheckoutAsync(request.Request, cancellationToken);
        return checkout;
    }
}
