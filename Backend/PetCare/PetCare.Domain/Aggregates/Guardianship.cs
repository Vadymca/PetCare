namespace PetCare.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;

/// <summary>
/// Represents a guardianship relationship between a user and an animal.
/// </summary>
public sealed class Guardianship : AggregateRoot
{
    private readonly List<GuardianshipDonation> donations = new();

    private Guardianship()
    {
    }

    private Guardianship(Guid userId, Guid animalId, DateTime startedAtUtc, DateTime? graceUntilUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор тварини не може бути порожнім.", nameof(animalId));
        }

        this.UserId = userId;
        this.AnimalId = animalId;
        this.StartDate = startedAtUtc;
        this.Status = GuardianshipStatus.RequiresPayment;
        this.GraceUntil = graceUntilUtc;
        this.CreatedAt = startedAtUtc;
        this.UpdatedAt = startedAtUtc;

        this.AddDomainEvent(new GuardianshipCreatedEvent(this.Id, userId, animalId));
    }

    /// <summary>Gets the user id who takes guardianship.</summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the current user associated with the context, or null if no user is set.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>Gets the animal id under guardianship.</summary>
    public Guid AnimalId { get; private set; }

    /// <summary>
    /// Gets the current animal associated with this instance.
    /// </summary>
    public Animal? Animal { get; private set; }

    /// <summary>Gets the start (creation) time in UTC.</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>Gets current guardianship status.</summary>
    public GuardianshipStatus Status { get; private set; }

    /// <summary>Gets deadline for first/next payment before auto-completion.</summary>
    public DateTime? GraceUntil { get; private set; }

    /// <summary>Gets created timestamp (UTC).</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Gets updated timestamp (UTC).</summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>Gets donations (links) associated with this guardianship.</summary>
    public IReadOnlyCollection<GuardianshipDonation> Donations => this.donations.AsReadOnly();

    /// <summary>
    /// Creates a guardianship in RequiresPayment with a grace period.
    /// </summary>
    /// <param name="userId">User who takes guardianship.</param>
    /// <param name="animalId">Animal under guardianship.</param>
    /// <param name="grace">Grace period for first payment.</param>
    /// <returns>New guardianship instance.</returns>
    public static Guardianship Create(Guid userId, Guid animalId, TimeSpan grace)
    {
        var now = DateTime.UtcNow;
        return new Guardianship(userId, animalId, now, now.Add(grace));
    }

    /// <summary>Marks first successful payment — activate guardianship.</summary>
    public void Activate()
    {
        if (this.Status == GuardianshipStatus.Completed)
        {
            throw new InvalidOperationException("Опіка вже завершена.");
        }

        if (this.Status == GuardianshipStatus.Active)
        {
            return;
        }

        this.Status = GuardianshipStatus.Active;
        this.GraceUntil = null;
        this.Touch();
        this.AddDomainEvent(new GuardianshipActivatedEvent(this.Id, this.UserId, this.AnimalId));
    }

    /// <summary>Payment missed/failed — require payment again and reset grace.</summary>
    /// <param name="grace">Grace period for next payment.</param>
    public void RequirePayment(TimeSpan grace)
    {
        if (this.Status == GuardianshipStatus.Completed)
        {
            throw new InvalidOperationException("Опіка вже завершена.");
        }

        this.Status = GuardianshipStatus.RequiresPayment;
        this.GraceUntil = DateTime.UtcNow.Add(grace);
        this.Touch();
        this.AddDomainEvent(new GuardianshipRequiresPaymentEvent(this.Id, this.UserId, this.AnimalId, this.GraceUntil!.Value));
    }

    /// <summary>Completes guardianship (on cancel or grace expiration).</summary>
    public void Complete()
    {
        if (this.Status == GuardianshipStatus.Completed)
        {
            return;
        }

        this.Status = GuardianshipStatus.Completed;
        this.GraceUntil = null;
        this.Touch();
        this.AddDomainEvent(new GuardianshipCompletedEvent(this.Id, this.UserId, this.AnimalId));
    }

    /// <summary>Links a donation to this guardianship.</summary>
    /// <param name="donationId">Donation identifier.</param>
    public void AddDonation(Guid donationId)
    {
        if (donationId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор транзакції не може бути порожнім.", nameof(donationId));
        }

        if (this.donations.Any(d => d.DonationId == donationId))
        {
            throw new InvalidOperationException("Цю транзакцію вже пов’язано з опікою.");
        }

        this.donations.Add(GuardianshipDonation.Create(this.Id, donationId));
        this.Touch();
        this.AddDomainEvent(new GuardianshipDonationAddedEvent(this.Id, donationId));
    }

    private void Touch() => this.UpdatedAt = DateTime.UtcNow;
}
