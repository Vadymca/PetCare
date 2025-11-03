namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Entities;

/// <summary>
/// Defines operations for managing payment methods, including retrieval, creation, update, and deletion functionality.
/// </summary>
/// <remarks>This interface provides asynchronous methods for working with payment methods in a system.
/// Implementations should ensure thread safety and handle cancellation requests appropriately via the provided <see
/// cref="CancellationToken"/> parameters. Methods may throw exceptions such as <see cref="ArgumentNullException"/>,
/// <see cref="ArgumentException"/>, or <see cref="InvalidOperationException"/> depending on invalid input or operation
/// state. Usage scenarios include listing available payment methods, retrieving details by identifier, and modifying or
/// removing payment methods as needed.</remarks>
public interface IPaymentMethodService
{
    /// <summary>
    /// Asynchronously retrieves all available payment methods.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list containing all payment methods. The list will be empty if no payment methods are available.</returns>
    Task<IReadOnlyList<PaymentMethod>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a payment method by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment method associated with
    /// the specified identifier, or <see langword="null"/> if no matching payment method is found.</returns>
    Task<PaymentMethod> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new payment method with the specified name.
    /// </summary>
    /// <param name="name">The name to assign to the new payment method. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created payment method.</returns>
    Task<PaymentMethod> CreateAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the name of an existing payment method identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to update.</param>
    /// <param name="newName">The new name to assign to the payment method. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see
    /// cref="PaymentMethod"/> instance.</returns>
    Task<PaymentMethod> UpdateAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the entity identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
