// <copyright file="IPaymentProcessor.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides functionality for processing and verifying payments.
/// </summary>
public interface IPaymentProcessor
{
    /// <summary>
    /// Initiates a payment for the specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user making the payment.</param>
    /// <param name="amount">The amount to be paid.</param>
    /// <param name="description">A description of the payment.</param>
    /// <returns>A task that resolves to the payment identifier.</returns>
    Task<string> ProcessPaymentAsync(Guid userId, decimal amount, string description);

    /// <summary>
    /// Checks whether the payment with the specified identifier was successful.
    /// </summary>
    /// <param name="paymentId">The identifier of the payment to verify.</param>
    /// <returns>A task that resolves to <c>true</c> if the payment was successful; otherwise, <c>false</c>.</returns>
    Task<bool> IsPaymentSuccessfulAsync(string paymentId);
}
