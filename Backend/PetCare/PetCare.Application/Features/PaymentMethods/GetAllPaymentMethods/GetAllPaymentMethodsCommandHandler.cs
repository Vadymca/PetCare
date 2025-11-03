namespace PetCare.Application.Features.PaymentMethods.GetAllPaymentMethods;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles retrieval of all payment methods.
/// </summary>
public sealed class GetAllPaymentMethodsCommandHandler
    : IRequestHandler<GetAllPaymentMethodsCommand, IReadOnlyList<PaymentMethodDto>>
{
    private readonly IPaymentMethodService paymentMethods;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllPaymentMethodsCommandHandler"/> class using the specified payment method.
    /// service.
    /// </summary>
    /// <param name="paymentMethods">The service used to retrieve and manage payment methods. Cannot be null.</param>
    public GetAllPaymentMethodsCommandHandler(IPaymentMethodService paymentMethods)
    {
        this.paymentMethods = paymentMethods;
    }

    /// <summary>
    /// Retrieves all available payment methods and returns them as a read-only list of data transfer objects.
    /// </summary>
    /// <param name="request">The command containing any parameters or context required to retrieve the payment methods.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="PaymentMethodDto"/> objects representing all available payment methods. The list
    /// will be empty if no payment methods are found.</returns>
    public async Task<IReadOnlyList<PaymentMethodDto>> Handle(
        GetAllPaymentMethodsCommand request,
        CancellationToken cancellationToken)
    {
        var list = await this.paymentMethods.GetAllAsync(cancellationToken);

        return list
            .Select(p => new PaymentMethodDto(p.Id, p.Name.Value))
            .ToList();
    }
}
