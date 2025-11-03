namespace PetCare.Application.Features.PaymentMethods.GetPaymentMethodById;

using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles retrieval of a payment method by its unique identifier.
/// </summary>
public sealed class GetPaymentMethodByIdCommandHandler
    : IRequestHandler<GetPaymentMethodByIdCommand, PaymentMethodDto>
{
    private readonly IPaymentMethodService paymentMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPaymentMethodByIdCommandHandler"/> class with the specified payment method.
    /// service.
    /// </summary>
    /// <param name="paymentMethods">The service used to retrieve and manage payment methods. Cannot be null.</param>
    public GetPaymentMethodByIdCommandHandler(IPaymentMethodService paymentMethods)
    {
        this.paymentMethods = paymentMethods;
    }

    /// <summary>
    /// Retrieves the payment method details for the specified identifier.
    /// </summary>
    /// <param name="request">The command containing the identifier of the payment method to retrieve.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="PaymentMethodDto"/> containing the details of the requested payment method.</returns>
    public async Task<PaymentMethodDto> Handle(GetPaymentMethodByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.paymentMethods.GetByIdAsync(request.Id, cancellationToken);
        return new PaymentMethodDto(entity.Id, entity.Name.Value);
    }
}
