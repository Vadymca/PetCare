namespace PetCare.Domain.Entities;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents a donation in the system.
/// </summary>
public sealed class Donation : BaseEntity
{
    private readonly List<AnimalAidDonation> animalAidLinks = new();

    private Donation()
    {
    }

    private Donation(
        Guid? userId,
        decimal amount,
        Guid? shelterId,
        Guid paymentMethodId,
        DonationStatus status,
        string? transactionId,
        string? purpose,
        bool recurring,
        bool anonymous,
        DateTime? donationDate,
        string? report)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Сума повинна бути більшою за 0.", nameof(amount));
        }

        this.UserId = userId;
        this.Amount = amount;
        this.ShelterId = shelterId;
        this.PaymentMethodId = paymentMethodId;
        this.Status = status;
        this.TransactionId = transactionId;
        this.Purpose = purpose;
        this.Recurring = recurring;
        this.Anonymous = anonymous;
        this.DonationDate = donationDate ?? DateTime.UtcNow;
        this.Report = report;

        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the amount of the donation.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Gets the current status of the donation.
    /// </summary>
    public DonationStatus Status { get; private set; }

    /// <summary>
    /// Gets the transaction identifier for the donation, if any. Can be null.
    /// </summary>
    public string? TransactionId { get; private set; }

    /// <summary>
    /// Gets the purpose of the donation, if any. Can be null.
    /// </summary>
    public string? Purpose { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the donation is recurring.
    /// </summary>
    public bool Recurring { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the donation is anonymous.
    /// </summary>
    public bool Anonymous { get; private set; }

    /// <summary>
    /// Gets the date and time when the donation was made.
    /// </summary>
    public DateTime DonationDate { get; private set; }

    /// <summary>
    /// Gets the report associated with the donation, if any. Can be null.
    /// </summary>
    public string? Report { get; private set; }

    /// <summary>
    /// Gets the date and time when the donation was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the donation was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user making the donation, if any. Can be null.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets navigation property for the user who made the donation.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter receiving the donation, if any. Can be null.
    /// </summary>
    public Guid? ShelterId { get; private set; }

    /// <summary>
    /// Gets navigation property for the shelter receiving the donation.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the payment method used for the donation.
    /// </summary>
    public Guid PaymentMethodId { get; private set; }

    /// <summary>
    /// Gets navigation property to the payment method.
    /// </summary>
    public PaymentMethod? PaymentMethod { get; private set; }

    /// <summary>
    /// Gets the list of AnimalAidRequest links associated with this donation.
    /// </summary>
    public IReadOnlyList<AnimalAidDonation> AnimalAidLinks => this.animalAidLinks.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Donation"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user making the donation, if any. Can be null.</param>
    /// <param name="amount">The amount of the donation.</param>
    /// <param name="shelterId">The unique identifier of the shelter receiving the donation, if any. Can be null.</param>
    /// <param name="paymentMethodId">The unique identifier of the payment method used for the donation.</param>
    /// <param name="status">The current status of the donation.</param>
    /// <param name="transactionId">The transaction identifier for the donation, if any. Can be null.</param>
    /// <param name="purpose">The purpose of the donation, if any. Can be null.</param>
    /// <param name="recurring">Indicates whether the donation is recurring. Defaults to false.</param>
    /// <param name="anonymous">Indicates whether the donation is anonymous. Defaults to false.</param>
    /// <param name="donationDate">The date and time when the donation was made. If null, defaults to the current UTC time.</param>
    /// <param name="report">The report associated with the donation, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Donation"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="amount"/> is less than or equal to zero.</exception>
    public static Donation Create(
        Guid? userId,
        decimal amount,
        Guid? shelterId,
        Guid paymentMethodId,
        DonationStatus status,
        string? transactionId = null,
        string? purpose = null,
        bool recurring = false,
        bool anonymous = false,
        DateTime? donationDate = null,
        string? report = null)
    {
        return new Donation(
           userId,
           amount,
           shelterId,
           paymentMethodId,
           status,
           transactionId,
           purpose,
           recurring,
           anonymous,
           donationDate,
           report);
    }

    /// <summary>
    /// Updates the report associated with the donation.
    /// </summary>
    /// <param name="report">The new report for the donation.</param>
    public void UpdateReport(string report)
    {
        this.Report = report;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the donation as completed and optionally updates the transaction identifier.
    /// </summary>
    /// <param name="transactionId">The new transaction identifier, if provided. If null or empty, the transaction identifier remains unchanged.</param>
    public void MarkAsCompleted(string? transactionId = null)
    {
        this.Status = DonationStatus.Completed;
        if (!string.IsNullOrWhiteSpace(transactionId))
        {
            this.TransactionId = transactionId;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the donation as failed.
    /// </summary>
    public void MarkAsFailed()
    {
        this.Status = DonationStatus.Failed;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the status of the donation.
    /// </summary>
    /// <param name="status">The new status of the donation.</param>
    public void SetStatus(DonationStatus status)
    {
        this.Status = status;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a link between this donation and an AnimalAidRequest.
    /// </summary>
    /// <param name="link">The link to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="link"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the link already exists.</exception>
    public void AddAnimalAidLink(AnimalAidDonation link)
    {
        if (link == null)
        {
            throw new ArgumentNullException(nameof(link), "Зв'язок не може бути null.");
        }

        if (this.animalAidLinks.Any(l => l.Id == link.Id))
        {
            throw new InvalidOperationException("Цей зв'язок вже додано.");
        }

        this.animalAidLinks.Add(link);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a link between this donation and an AnimalAidRequest.
    /// </summary>
    /// <param name="linkId">The ID of the link to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown if the link is not found.</exception>
    public void RemoveAnimalAidLink(Guid linkId)
    {
        var link = this.animalAidLinks.FirstOrDefault(l => l.Id == linkId);
        if (link == null)
        {
            throw new InvalidOperationException("Зв'язок не знайдено.");
        }

        this.animalAidLinks.Remove(link);
        this.UpdatedAt = DateTime.UtcNow;
    }
}
