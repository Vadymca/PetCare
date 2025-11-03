namespace PetCare.Application.Features.PaymentMethods.DeletePaymentMethod;

using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles deletion of a payment method by its unique identifier.
/// </summary>
public sealed class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand>
{
    private readonly IPaymentMethodService paymentMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePaymentMethodCommandHandler"/> class with the specified payment method.
    /// service.
    /// </summary>
    /// <param name="paymentMethods">The service used to manage payment methods. Cannot be null.</param>
    public DeletePaymentMethodCommandHandler(IPaymentMethodService paymentMethods)
    {
        this.paymentMethods = paymentMethods;
    }

    /// <summary>
    /// Handles a request to delete a payment method identified by the specified command.
    /// </summary>
    /// <param name="request">The command containing the identifier of the payment method to delete.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        await this.paymentMethods.DeleteAsync(request.Id, cancellationToken);
    }
}
