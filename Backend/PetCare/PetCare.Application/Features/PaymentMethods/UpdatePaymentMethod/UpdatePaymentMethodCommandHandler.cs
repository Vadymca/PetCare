namespace PetCare.Application.Features.PaymentMethods.UpdatePaymentMethod;

using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles updating of a payment method's name.
/// </summary>
public sealed class UpdatePaymentMethodCommandHandler
    : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethodDto>
{
    private readonly IPaymentMethodService paymentMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentMethodCommandHandler"/> class with the specified payment method.
    /// service.
    /// </summary>
    /// <param name="paymentMethods">The service used to manage and update payment methods. Cannot be null.</param>
    public UpdatePaymentMethodCommandHandler(IPaymentMethodService paymentMethods)
    {
        this.paymentMethods = paymentMethods;
    }

    /// <summary>
    /// Updates the payment method with the specified information and returns the updated payment method details.
    /// </summary>
    /// <param name="request">The command containing the identifier of the payment method to update and the new name to assign. Cannot be
    /// null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A <see cref="PaymentMethodDto"/> representing the updated payment method details.</returns>
    public async Task<PaymentMethodDto> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.paymentMethods.UpdateAsync(request.Id, request.NewName, cancellationToken);
        return new PaymentMethodDto(entity.Id, entity.Name.Value);
    }
}
