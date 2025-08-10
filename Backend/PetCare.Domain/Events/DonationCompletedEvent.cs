namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when a donation is successfully completed.
/// </summary>
public sealed record DonationCompletedEvent(Guid donationId, decimal amount)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
