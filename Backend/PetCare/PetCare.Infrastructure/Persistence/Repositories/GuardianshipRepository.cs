namespace PetCare.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Provides data access and query operations for guardianship entities, including retrieval, listing, and association
/// with related data such as users, animals, and donations.
/// </summary>
/// <remarks>This repository extends generic repository functionality with guardianship-specific queries and
/// operations. It supports asynchronous methods for retrieving guardianship records with related details, filtering by
/// user or animal, and managing donation links. All methods are designed for use with Entity Framework Core and are
/// intended to be used within the application's data access layer.</remarks>
public sealed class GuardianshipRepository : GenericRepository<Guardianship>, IGuardianshipRepository
{
    private readonly AppDbContext db;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuardianshipRepository"/> class using the specified database context.
    /// </summary>
    /// <param name="context">The AppDbContext instance to be used for data access operations. Cannot be null.</param>
    public GuardianshipRepository(AppDbContext context)
        : base(context)
    {
        this.db = context;
    }

    /// <summary>
    /// Asynchronously retrieves a guardianship entity by its unique identifier, including related user, animal, and
    /// donation details.
    /// </summary>
    /// <remarks>The returned guardianship includes associated user, animal, and donation information. The
    /// entity is not tracked by the context.</remarks>
    /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity with related
    /// details if found; otherwise, null.</returns>
    public async Task<Guardianship?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await this.db.Set<Guardianship>()
            .AsNoTracking()
            .Include(g => g.User)
            .Include(g => g.Animal)
            .Include(g => g.Donations)
                .ThenInclude(gd => gd.Donation)
            .FirstOrDefaultAsync(g => g.Id == id, ct);
    }

    /// <summary>
    /// Retrieves a guardianship entity with the specified identifier for update operations.
    /// </summary>
    /// <remarks>The returned entity is tracked by the context, allowing changes to be detected and persisted.
    /// No related entities are included by default.</remarks>
    /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity if found;
    /// otherwise, null.</returns>
    public async Task<Guardianship?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        // Tracked entity (no includes by default — швидко та без зайвих графів)
        return await this.db.Set<Guardianship>()
            .FirstOrDefaultAsync(g => g.Id == id, ct);
    }

    /// <summary>
    /// Asynchronously retrieves the active guardianship record for the specified user and animal, if one exists.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose guardianship is to be retrieved.</param>
    /// <param name="animalId">The unique identifier of the animal associated with the guardianship.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the active guardianship for the
    /// specified user and animal, or null if no active guardianship exists.</returns>
    public async Task<Guardianship?> GetActiveByUserAndAnimalAsync(Guid userId, Guid animalId, CancellationToken ct = default)
    {
        return await this.db.Set<Guardianship>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
            g =>
                g.UserId == userId &&
                g.AnimalId == animalId &&
                g.Status == GuardianshipStatus.Active,
            ct);
    }

    /// <summary>
    /// Determines whether there is an active guardianship for the specified user and animal.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to check for an active guardianship.</param>
    /// <param name="animalId">The unique identifier of the animal to check for an active guardianship.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if an active
    /// guardianship exists for the specified user and animal; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> ExistsActiveByUserAndAnimalAsync(Guid userId, Guid animalId, CancellationToken ct = default)
    {
        return await this.db.Set<Guardianship>()
            .AsNoTracking()
            .AnyAsync(
            g =>
                g.UserId == userId &&
                g.AnimalId == animalId &&
                g.Status == GuardianshipStatus.Active,
            ct);
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianship records associated with the specified user, optionally
    /// filtered by guardianship status.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose guardianship records are to be retrieved.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If specified, only guardianships with the given
    /// status are returned.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records for the specified user, ordered by start date in descending order.</returns>
    public async Task<IReadOnlyList<Guardianship>> ListByUserAsync(Guid userId, GuardianshipStatus? status = null, CancellationToken ct = default)
    {
        var q = this.db.Set<Guardianship>()
            .AsNoTracking()
            .Include(g => g.Animal)
            .Where(g => g.UserId == userId);

        if (status is not null)
        {
            q = q.Where(g => g.Status == status.Value);
        }

        return await q
            .OrderByDescending(g => g.StartDate)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianship records associated with the specified animal,
    /// optionally filtered by guardianship status.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal for which to retrieve guardianship records.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If specified, only guardianships with the given
    /// status are returned.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records for the specified animal, ordered by start date in descending order. The list is empty if no matching
    /// records are found.</returns>
    public async Task<IReadOnlyList<Guardianship>> ListByAnimalAsync(Guid animalId, GuardianshipStatus? status = null, CancellationToken ct = default)
    {
        var q = this.db.Set<Guardianship>()
            .AsNoTracking()
            .Include(g => g.User)
            .Where(g => g.AnimalId == animalId);

        if (status is not null)
        {
            q = q.Where(g => g.Status == status.Value);
        }

        return await q
            .OrderByDescending(g => g.StartDate)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianships that have expired and require payment as of the
    /// specified UTC date and time.
    /// </summary>
    /// <param name="utcNow">The current date and time in UTC used to determine which guardianships have expired. Guardianships with a grace
    /// period ending on or before this value are included.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianships
    /// with a status of RequiresPayment and a grace period that has expired as of the specified time. The list is empty
    /// if no such guardianships exist.</returns>
    public async Task<IReadOnlyList<Guardianship>> ListExpiredRequiresPaymentAsync(DateTime utcNow, CancellationToken ct = default)
    {
        return await this.db.Set<Guardianship>()
            .AsNoTracking()
            .Where(g =>
                g.Status == GuardianshipStatus.RequiresPayment &&
                g.GraceUntil != null &&
                g.GraceUntil <= utcNow)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves a read-only list of guardianships that require payment within the specified UTC date and time range.
    /// </summary>
    /// <param name="fromUtc">The start of the UTC date and time range to search for guardianships requiring payment. Only guardianships with
    /// a grace period ending on or after this value are included.</param>
    /// <param name="toUtc">The end of the UTC date and time range to search for guardianships requiring payment. Only guardianships with a
    /// grace period ending on or before this value are included.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of guardianships that require payment and have a grace period ending within the specified
    /// range. The list is ordered by the grace period end date.</returns>
    public async Task<IReadOnlyList<Guardianship>> ListRequiresPaymentWithinAsync(DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
    {
        return await this.db.Set<Guardianship>()
            .AsNoTracking()
            .Where(g =>
                g.Status == GuardianshipStatus.RequiresPayment &&
                g.GraceUntil != null &&
                g.GraceUntil >= fromUtc &&
                g.GraceUntil <= toUtc)
            .OrderBy(g => g.GraceUntil)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Asynchronously creates a link between a guardianship and a donation if one does not already exist.
    /// </summary>
    /// <remarks>If a link between the specified guardianship and donation already exists, the method
    /// completes without making changes. This operation is idempotent.</remarks>
    /// <param name="guardianshipId">The unique identifier of the guardianship to associate with the donation.</param>
    /// <param name="donationId">The unique identifier of the donation to associate with the guardianship.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task LinkDonationAsync(Guid guardianshipId, Guid donationId, CancellationToken ct = default)
    {
        // idempotent link create
        var exists = await this.db.Set<GuardianshipDonation>()
            .AsNoTracking()
            .AnyAsync(x => x.GuardianshipId == guardianshipId && x.DonationId == donationId, ct);

        if (exists)
        {
            return;
        }

        this.db.Set<GuardianshipDonation>().Add(GuardianshipDonation.Create(guardianshipId, donationId));
        await this.db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Cancels the payment subscription associated with the specified guardianship asynchronously, if one exists.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship for which the subscription should be canceled.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous cancel operation. The task completes when the subscription has been
    /// canceled and changes have been saved.</returns>
    public async Task CancelSubscriptionAsync(Guid guardianshipId, CancellationToken ct = default)
    {
        var subscription = await this.db.PaymentSubscriptions
            .FirstOrDefaultAsync(s => s.ScopeType == SubscriptionScope.Guardianship && s.ScopeId == guardianshipId, ct);

        if (subscription is null)
        {
            return;
        }

        subscription.Cancel();
        await this.db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Asynchronously adds a new donation record to the database.
    /// </summary>
    /// <remarks>The changes are persisted to the database upon successful completion of the operation. If the
    /// cancellation token is triggered before completion, the operation is canceled and no changes are saved.</remarks>
    /// <param name="donation">The donation entity to add. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddDonationAsync(Donation donation, CancellationToken ct = default)
    {
        this.db.Set<Donation>().Add(donation);
        await this.db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Retrieves the unique identifier of a payment method by its provider name, ensuring that the payment method
    /// exists.
    /// </summary>
    /// <param name="provider">The name of the payment provider to search for. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The unique identifier of the payment method associated with the specified provider.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a payment method with the specified provider name does not exist.</exception>
    public async Task<Guid> RequirePaymentMethodIdByProviderAsync(string provider, CancellationToken cancellationToken = default)
    {
        var pm = await this.db.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.Value == provider, cancellationToken);

        if (pm is null)
        {
            throw new InvalidOperationException($"Метод оплати '{provider}' не знайдено.");
        }

        return pm.Id;
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations made by the specified user, ordered by most recent
    /// donation date.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose donations are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/> objects representing the user's donations, ordered from most recent
    /// to oldest. Returns an empty list if the user has not made any donations.</returns>
    public async Task<IReadOnlyList<Donation>> GetUserDonationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.db.Donations
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously adds a new payment subscription to the database.
    /// </summary>
    /// <param name="subscription">The payment subscription to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddSubscriptionAsync(PaymentSubscription subscription, CancellationToken cancellationToken = default)
    {
        this.db.PaymentSubscriptions.Add(subscription);
        await this.db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cancels an active payment subscription identified by the specified provider subscription ID.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier of the subscription as assigned by the external payment provider. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a subscription with the specified provider subscription ID does not exist.</exception>
    public async Task CancelSubscriptionByProviderIdAsync(string providerSubscriptionId, CancellationToken cancellationToken = default)
    {
        var sub = await this.db.PaymentSubscriptions
            .FirstOrDefaultAsync(s => s.ProviderSubscriptionId == providerSubscriptionId, cancellationToken)
            ?? throw new InvalidOperationException("Підписку не знайдено.");

        sub.Cancel();
        await this.db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cancels all active payment subscriptions within the specified scope and scope identifier asynchronously.
    /// </summary>
    /// <remarks>Only subscriptions with an active status matching the provided scope and scope identifier are
    /// cancelled. Changes are persisted to the database upon completion.</remarks>
    /// <param name="scope">The type of subscription scope to filter subscriptions for cancellation.</param>
    /// <param name="scopeId">The unique identifier of the scope. If null, no subscriptions will be matched.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    public async Task CancelSubscriptionsByScopeAsync(SubscriptionScope scope, Guid? scopeId, CancellationToken cancellationToken = default)
    {
        var subs = await this.db.PaymentSubscriptions
            .Where(s => s.ScopeType == scope && s.ScopeId == scopeId && s.Status == SubscriptionStatus.Active)
            .ToListAsync(cancellationToken);

        foreach (var s in subs)
        {
            s.Cancel();
        }

        await this.db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves the list of payment subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of payment subscriptions for the specified user, ordered by creation date in descending order.
    /// Returns an empty list if the user has no subscriptions.</returns>
    public async Task<IReadOnlyList<PaymentSubscription>> GetUserSubscriptionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.db.PaymentSubscriptions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a list of active payment subscriptions for the specified user, along with the date of the next
    /// expected charge for each subscription.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose active payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of tuples, each containing a payment subscription and the date of its next expected charge. The
    /// date may be null if no upcoming charge is scheduled.</returns>
    public async Task<IReadOnlyList<(PaymentSubscription Subscription, DateTime? NextChargeAt)>> GetUserExpectedChargesAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var items = await this.db.PaymentSubscriptions
            .AsNoTracking()
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active)
            .Select(s => new { s, s.NextChargeAt })
            .ToListAsync(cancellationToken);

        return items.Select(x => (x.s, x.NextChargeAt)).ToList();
    }

    /// <summary>
    /// Asynchronously retrieves a payment subscription that matches the specified provider subscription identifier.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier assigned to the subscription by the external payment provider. Cannot be null.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching PaymentSubscription if
    /// found; otherwise, null.</returns>
    public async Task<PaymentSubscription?> FindByProviderSubscriptionIdAsync(
    string providerSubscriptionId,
    CancellationToken cancellationToken = default)
    {
        return await this.db.PaymentSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProviderSubscriptionId == providerSubscriptionId, cancellationToken);
    }

    /// <summary>
    /// Retrieves a read-only list of all payment subscriptions in the system.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A read-only list of <see cref="PaymentSubscription"/> objects representing all existing subscriptions.
    /// Returns an empty list if no subscriptions exist.
    /// </returns>
    public async Task<IReadOnlyList<PaymentSubscription>> ListAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await this.db.PaymentSubscriptions
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all donations ordered by most recent first.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IReadOnlyList<Donation>> ListAllDonationsAsync(CancellationToken cancellationToken = default)
    {
        return await this.db.Donations
            .AsNoTracking()
            .Include(d => d.User)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves donations filtered by a specific project (AidRequest, Shelter, etc.).
    /// </summary>
    /// <param name="projectId">The unique identifier of the project for which to retrieve donations.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IReadOnlyList<Donation>> ListDonationsByProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await this.db.Donations
            .AsNoTracking()
            .Include(d => d.User)
            .Where(d => d.TargetEntity == "AnimalAidRequest" && d.TargetEntityId == projectId)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all payment methods ordered alphabetically by name.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A read-only list of payment methods.</returns>
    public async Task<IReadOnlyList<PaymentMethod>> ListAllPaymentMethodsAsync(CancellationToken cancellationToken = default)
    {
        return await this.db.PaymentMethods
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a payment method by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the payment method.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The matching payment method or null if not found.</returns>
    public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.db.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a payment method by its name.
    /// </summary>
    /// <param name="name">The name of the payment method.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The matching payment method or null if not found.</returns>
    public async Task<PaymentMethod?> GetPaymentMethodByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var vo = Name.Create(name);

        return await this.db.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == vo, cancellationToken);
    }

    /// <summary>
    /// Adds a new payment method to the database.
    /// </summary>
    /// <param name="method">The payment method to add.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddPaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default)
    {
        await this.db.PaymentMethods.AddAsync(method);
        await this.db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing payment method in the database.
    /// </summary>
    /// <param name="method">The payment method to update.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdatePaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default)
    {
        this.db.PaymentMethods.Update(method);
        await this.db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a payment method from the database.
    /// </summary>
    /// <param name="method">The payment method to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeletePaymentMethodAsync(PaymentMethod method, CancellationToken cancellationToken = default)
    {
        this.db.PaymentMethods.Remove(method);
        await this.db.SaveChangesAsync(cancellationToken);
    }
}
