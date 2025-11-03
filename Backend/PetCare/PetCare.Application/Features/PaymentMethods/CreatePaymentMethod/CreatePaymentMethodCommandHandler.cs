namespace PetCare.Application.Features.PaymentMethods.CreatePaymentMethod;

using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles creation of a new payment method.
/// </summary>
public sealed class CreatePaymentMethodCommandHandler
    : IRequestHandler<CreatePaymentMethodCommand, PaymentMethodDto>
{
    private readonly IPaymentMethodService paymentMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePaymentMethodCommandHandler"/> class with the specified payment method.
    /// service.
    /// </summary>
    /// <param name="paymentMethods">The service used to manage and process payment methods. Cannot be null.</param>
    public CreatePaymentMethodCommandHandler(IPaymentMethodService paymentMethods)
    {
        this.paymentMethods = paymentMethods;
    }

    /// <summary>
    /// Creates a new payment method using the specified command and returns its details.
    /// </summary>
    /// <param name="request">The command containing the information required to create the payment method. Must not be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="PaymentMethodDto"/> representing the newly created payment method.</returns>
    public async Task<PaymentMethodDto> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.paymentMethods.CreateAsync(request.Name, cancellationToken);
        return new PaymentMethodDto(entity.Id, entity.Name.Value);
    }
}
