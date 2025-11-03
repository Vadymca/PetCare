namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Application service that handles PaymentMethod logic and delegates persistence to the GuardianshipRepository.
/// </summary>
public sealed class PaymentMethodService : IPaymentMethodService
{
    private readonly IGuardianshipRepository guardianships;
    private readonly ILogger<PaymentMethodService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentMethodService"/> class with the specified guardianship repository and.
    /// logger.
    /// </summary>
    /// <param name="guardianships">The repository used to access and manage guardianship data required by the service. Cannot be null.</param>
    /// <param name="logger">The logger used for recording diagnostic and operational information for the service. Cannot be null.</param>
    public PaymentMethodService(
        IGuardianshipRepository guardianships,
        ILogger<PaymentMethodService> logger)
    {
        this.guardianships = guardianships;
        this.logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves all available payment methods.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list containing all payment methods. The list will be empty if no payment methods are available.</returns>
    public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.guardianships.ListAllPaymentMethodsAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a payment method by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the payment method associated with
    /// the specified identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a payment method with the specified identifier does not exist.</exception>
    public async Task<PaymentMethod> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var method = await this.guardianships.GetPaymentMethodByIdAsync(id, cancellationToken);
        if (method is null)
        {
            throw new InvalidOperationException($"Метод оплати з Id '{id}' не знайдено.");
        }

        return method;
    }

    /// <summary>
    /// Creates a new payment method with the specified name asynchronously.
    /// </summary>
    /// <param name="name">The name of the payment method to create. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created payment method.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a payment method with the specified name already exists.</exception>
    public async Task<PaymentMethod> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var vo = Name.Create(name);

        var existing = await this.guardianships.GetPaymentMethodByNameAsync(vo.Value, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException($"Метод оплати '{vo.Value}' вже існує.");
        }

        var entity = PaymentMethod.Create(vo);
        await this.guardianships.AddPaymentMethodAsync(entity, cancellationToken);

        this.logger.LogInformation("✅ Створено новий метод оплати: {Name}", vo.Value);
        return entity;
    }

    /// <summary>
    /// Asynchronously updates the name of an existing payment method identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to update.</param>
    /// <param name="newName">The new name to assign to the payment method. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated payment method.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a payment method with the specified <paramref name="id"/> does not exist.</exception>
    public async Task<PaymentMethod> UpdateAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var entity = await this.guardianships.GetPaymentMethodByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Метод оплати з Id '{id}' не знайдено.");

        var vo = Name.Create(newName);
        var existing = await this.guardianships.GetPaymentMethodByNameAsync(vo.Value, cancellationToken);

        if (existing is not null && existing.Id != entity.Id)
        {
            throw new InvalidOperationException($"Метод оплати '{vo.Value}' вже існує.");
        }

        entity.Rename(vo.Value);
        await this.guardianships.UpdatePaymentMethodAsync(entity, cancellationToken);

        this.logger.LogInformation("✏️ Оновлено метод оплати {Id}: {NewName}", id, vo.Value);
        return entity;
    }

    /// <summary>
    /// Asynchronously deletes the payment method identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a payment method with the specified <paramref name="id"/> does not exist.</exception>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await this.guardianships.GetPaymentMethodByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Метод оплати з Id '{id}' не знайдено.");

        await this.guardianships.DeletePaymentMethodAsync(entity, cancellationToken);
        this.logger.LogInformation("Видалено метод оплати {Id}", id);
    }
}
