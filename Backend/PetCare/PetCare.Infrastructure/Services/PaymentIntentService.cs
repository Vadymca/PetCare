namespace PetCare.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Provides high-level operations for managing <see cref="PaymentIntent"/> aggregates.
/// </summary>
public sealed class PaymentIntentService : IPaymentIntentService
{
    private readonly IPaymentIntentRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentIntentService"/> class.
    /// </summary>
    /// <param name="repository">Repository for payment intents.</param>
    public PaymentIntentService(IPaymentIntentRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc/>
    public async Task<PaymentIntent> CreateLiqPayIntentAsync(
        SubscriptionScope? scopeType,
        Guid? scopeId,
        Guid? userId,
        decimal amount,
        string currency,
        bool isRecurring,
        bool anonymous,
        CancellationToken cancellationToken = default)
    {
        var intent = PaymentIntent.CreateForLiqPay(
            scopeType,
            scopeId,
            userId,
            amount,
            currency,
            isRecurring,
            anonymous);

        await this.repository.AddAsync(intent, cancellationToken);
        return intent;
    }

    /// <inheritdoc/>
    public Task<PaymentIntent?> GetByExternalOrderIdAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default)
    {
        return this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task MarkSucceededAsync(
        string externalOrderId,
        string? providerPaymentId,
        CancellationToken cancellationToken = default)
    {
        var intent = await this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Платіжний намір не знайдено.");

        intent.MarkSucceeded(providerPaymentId);

        await this.repository.UpdateAsync(intent, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task MarkFailedAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default)
    {
        var intent = await this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Платіжний намір не знайдено.");

        intent.MarkFailed();

        await this.repository.UpdateAsync(intent, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AttachDonationAsync(
        string externalOrderId,
        Guid donationId,
        CancellationToken cancellationToken = default)
    {
        var intent = await this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Платіжний намір не знайдено.");

        intent.AttachDonation(donationId);

        await this.repository.UpdateAsync(intent, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AttachSubscriptionAsync(
        string externalOrderId,
        Guid subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var intent = await this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Платіжний намір не знайдено.");

        intent.AttachSubscription(subscriptionId);

        await this.repository.UpdateAsync(intent, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AttachGuardianshipAsync(
        string externalOrderId,
        Guid guardianshipId,
        CancellationToken cancellationToken = default)
    {
        var intent = await this.repository.FindByExternalOrderIdAsync(externalOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Платіжний намір не знайдено.");

        intent.AttachGuardianship(guardianshipId);

        await this.repository.UpdateAsync(intent, cancellationToken);
    }
}
