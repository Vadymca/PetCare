namespace PetCare.Domain.Entities;

using PetCare.Domain.Common;

/// <summary>
/// Represents a value object that associates a donation with an animal aid request.
/// </summary>
public sealed class AnimalAidDonation : BaseEntity
{
    private AnimalAidDonation()
    {
    }

    private AnimalAidDonation(Guid donationId, Guid animalAidRequestId, DateTime createdAt)
    {
        if (donationId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор донації не може бути порожнім.", nameof(donationId));
        }

        if (animalAidRequestId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор запиту на допомогу тварині не може бути порожнім.", nameof(animalAidRequestId));
        }

        this.DonationId = donationId;
        this.AnimalAidRequestId = animalAidRequestId;
        this.CreatedAt = createdAt;
    }

    /// <summary>
    /// Gets the unique identifier of the donation.
    /// </summary>
    public Guid DonationId { get; private set; }

    /// <summary>
    /// Gets the donation associated with this entity.
    /// </summary>
    public Donation? Donation { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal aid request.
    /// </summary>
    public Guid AnimalAidRequestId { get; private set; }

    /// <summary>
    /// Gets the animal aid request associated with this entity.
    /// </summary>
    public AnimalAidRequest? AnimalAidRequest { get; private set; }

    /// <summary>
    /// Gets the date and time when the association was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="AnimalAidDonation"/> instance with the specified parameters.
    /// </summary>
    /// <param name="donationId">The unique identifier of the donation.</param>
    /// <param name="animalAidRequestId">The unique identifier of the animal aid request.</param>
    /// <returns>A new instance of <see cref="AnimalAidDonation"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="donationId"/> or <paramref name="animalAidRequestId"/> is empty.</exception>
    public static AnimalAidDonation Create(Guid donationId, Guid animalAidRequestId)
    {
        return new AnimalAidDonation(
            donationId,
            animalAidRequestId,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Returns the components used to determine equality for the <see cref="AnimalAidDonation"/> instance.
    /// </summary>
    /// <returns>An enumerable of objects representing the equality components.</returns>
}
