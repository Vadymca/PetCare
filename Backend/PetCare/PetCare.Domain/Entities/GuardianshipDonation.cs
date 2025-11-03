namespace PetCare.Domain.Entities;

using System;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;

/// <summary>
/// Join entity linking a donation to a guardianship.
/// </summary>
public sealed class GuardianshipDonation : BaseEntity
{
    private GuardianshipDonation()
    {
    }

    private GuardianshipDonation(Guid guardianshipId, Guid donationId)
    {
        this.GuardianshipId = guardianshipId;
        this.DonationId = donationId;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = this.CreatedAt;
    }

    /// <summary>Gets guardianship id.</summary>
    public Guid GuardianshipId { get; private set; }

    /// <summary>
    /// Gets the guardianship status associated with the entity, if available.
    /// </summary>
    public Guardianship? Guardianship { get; private set; }

    /// <summary>Gets donation id.</summary>
    public Guid DonationId { get; private set; }

   /// <summary>
   /// Gets the donation associated with the current instance.
   /// </summary>
    public Donation? Donation { get; private set; }

    /// <summary>Gets created timestamp (UTC).</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Gets updated timestamp (UTC).</summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new instance of the GuardianshipDonation class using the specified guardianship and donation
    /// identifiers.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship associated with the donation.</param>
    /// <param name="donationId">The unique identifier of the donation to be linked to the guardianship.</param>
    /// <returns>A GuardianshipDonation object initialized with the provided guardianship and donation identifiers.</returns>
    public static GuardianshipDonation Create(Guid guardianshipId, Guid donationId)
        => new(guardianshipId, donationId);
}
