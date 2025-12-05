namespace PetCare.Infrastructure.Persistence.Repositories;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;

/// <summary>
/// Provides data access operations for <see cref="PaymentIntent"/> entities, including methods to retrieve payment
/// intents by external order ID or provider payment ID.
/// </summary>
/// <remarks>This repository encapsulates logic for querying and managing payment intent records in the
/// application's database. It extends <see cref="GenericRepository{PaymentIntent}"/> and implements <see
/// cref="IPaymentIntentRepository"/>, providing specialized methods for payment intent lookups. Instances of this class
/// are typically used within the application's data access layer.</remarks>
public sealed class PaymentIntentRepository
: GenericRepository<PaymentIntent>, IPaymentIntentRepository
{
    private readonly AppDbContext db;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentIntentRepository"/> class.
    /// </summary>
    /// <param name="db">Application database context.</param>
    public PaymentIntentRepository(AppDbContext db)
        : base(db)
    {
        this.db = db;
    }

    /// <inheritdoc/>
    public async Task<PaymentIntent?> FindByExternalOrderIdAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalOrderId))
        {
            throw new ArgumentNullException(nameof(externalOrderId));
        }

        return await this.db.PaymentIntents
            .Include(pi => pi.Donation)
            .Include(pi => pi.Subscription)
            .Include(pi => pi.Guardianship)
                .ThenInclude(g => g!.Animal)
            .FirstOrDefaultAsync(
                pi => pi.ExternalOrderId == externalOrderId,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PaymentIntent?> FindByProviderPaymentIdAsync(
        string providerPaymentId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(providerPaymentId))
        {
            throw new ArgumentNullException(nameof(providerPaymentId));
        }

        return await this.db.PaymentIntents
            .Include(pi => pi.Donation)
            .Include(pi => pi.Subscription)
            .Include(pi => pi.Guardianship)
            .FirstOrDefaultAsync(
                pi => pi.ProviderPaymentId == providerPaymentId,
                cancellationToken);
    }

    /// <inheritdoc/>
    public Task<List<PaymentIntent>> GetByGuardianshipIdAsync(
    Guid guardianshipId,
    CancellationToken ct = default)
    {
        return this.db.PaymentIntents
            .Where(pi => pi.GuardianshipId == guardianshipId)
            .ToListAsync(ct);
    }
}
