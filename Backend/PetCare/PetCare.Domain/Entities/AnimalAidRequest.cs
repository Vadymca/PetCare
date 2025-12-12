namespace PetCare.Domain.Entities;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a request for aid related to animals in the system.
/// </summary>
public sealed class AnimalAidRequest : AggregateRoot
{
    private readonly List<string> photos = new();
    private readonly List<AnimalAidDonation> donations = new();
    private decimal collectedAmount;

    private AnimalAidRequest()
    {
        this.Title = null!;
        this.Slug = null!;
        this.ShortDescription = string.Empty;
        this.ContactPhone = null;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    private AnimalAidRequest(
        Guid? userId,
        Guid? shelterId,
        Title title,
        string shortDescription,
        string? description,
        AidCategory category,
        AidStatus status,
        decimal? estimatedCost,
        PhoneNumber? contactPhone,
        Slug slug,
        bool isUrgent,
        List<string>? photos)
    {
        if (estimatedCost is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedCost), "Орієнтовна вартість має бути невід'ємною");
        }

        if (string.IsNullOrWhiteSpace(shortDescription))
        {
            throw new ArgumentException("Короткий опис не може бути порожнім.", nameof(shortDescription));
        }

        this.UserId = userId;
        this.ShelterId = shelterId;
        this.Title = title;
        this.ShortDescription = shortDescription;
        this.Description = description;
        this.Category = category;
        this.Status = status;
        this.EstimatedCost = estimatedCost;
        this.ContactPhone = contactPhone;
        this.Slug = slug;
        this.IsUrgent = isUrgent;
        this.photos = photos ?? new List<string>();
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the title of the aid request.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the short description for list cards.
    /// </summary>
    public string ShortDescription { get; private set; }

    /// <summary>
    /// Gets the description of the aid request, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the category of the aid request.
    /// </summary>
    public AidCategory Category { get; private set; }

    /// <summary>
    /// Gets the current status of the aid request.
    /// </summary>
    public AidStatus Status { get; private set; }

    /// <summary>
    /// Gets the estimated cost of the aid request, if known. Can be null.
    /// </summary>
    public decimal? EstimatedCost { get; private set; }

    /// <summary>
    /// Gets the contact phone for non-donation aid. Can be null.
    /// </summary>
    public PhoneNumber? ContactPhone { get; private set; }

    /// <summary>
    /// Gets the SEO-friendly slug.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the request is urgent.
    /// </summary>
    public bool IsUrgent { get; private set; }

    /// <summary>
    /// Gets the total collected amount.
    /// </summary>
    public decimal CollectedAmount => this.collectedAmount;

    /// <summary>
    /// Gets the list of photo URLs for the aid request.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the date and time when the aid request was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the aid request was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the list of donations linked to this AnimalAidRequest.
    /// </summary>
    public IReadOnlyList<AnimalAidDonation> Donations => this.donations.AsReadOnly();

    /// <summary>
    /// Gets the unique identifier of the user making the request, if any. Can be null.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the user who made the aid request, if any.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter associated with the request, if any. Can be null.
    /// </summary>
    public Guid? ShelterId { get; private set; }

    /// <summary>
    /// Gets the shelter associated with the aid request, if any.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Creates a new <see cref="AnimalAidRequest"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user making the request, if any. Can be null.</param>
    /// <param name="shelterId">The unique identifier of the shelter associated with the request, if any. Can be null.</param>
    /// <param name="title">The title of the aid request.</param>
    /// <param name="shortDescription">Short description for lists/cards.</param>
    /// <param name="description">The description of the aid request, if any. Can be null.</param>
    /// <param name="category">The category of the aid request.</param>
    /// <param name="status">The current status of the aid request.</param>
    /// <param name="estimatedCost">The estimated cost of the aid request, if known. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the aid request. Can be null.</param>
    /// <param name="contactPhone">Optional contact phone (string). If null or whitespace => no phone.</param>
    /// <param name="isUrgent">Indicates whether the request is urgent.</param>
    /// <returns>A new instance of <see cref="AnimalAidRequest"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="estimatedCost"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to <see cref="Title.Create"/>.</exception>
    public static AnimalAidRequest Create(
        Guid? userId,
        Guid? shelterId,
        string title,
        string shortDescription,
        string? description,
        AidCategory category,
        AidStatus status,
        decimal? estimatedCost,
        List<string>? photos = null,
        string? contactPhone = null,
        bool isUrgent = false)
    {
        var phoneVo = string.IsNullOrWhiteSpace(contactPhone) ? null : PhoneNumber.Create(contactPhone);
        var slug = Slug.Create(title);

        var entity = new AnimalAidRequest(
            userId,
            shelterId,
            Title.Create(title),
            shortDescription,
            description,
            category,
            status,
            estimatedCost,
            phoneVo,
            slug,
            isUrgent,
            photos);

        return entity;
    }

    /// <summary>
    /// Updates the status of the aid request.
    /// </summary>
    /// <param name="status">The new status of the aid request.</param>
    public void UpdateStatus(AidStatus status)
    {
        this.Status = status;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the estimated cost of the aid request.
    /// </summary>
    /// <param name="newCost">The new estimated cost of the aid request. Can be null.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="newCost"/> is negative.</exception>
    public void UpdateEstimatedCost(decimal? newCost)
    {
        if (newCost is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newCost), "Вартість повинна бути невід'ємною.");
        }

        this.EstimatedCost = newCost;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the short description.
    /// </summary>
    /// <param name="newShortDescription">New short description.</param>
    public void UpdateShortDescription(string newShortDescription)
    {
        if (string.IsNullOrWhiteSpace(newShortDescription))
        {
            throw new ArgumentException("Короткий опис не може бути порожнім.", nameof(newShortDescription));
        }

        this.ShortDescription = newShortDescription;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates or clears the contact phone.
    /// </summary>
    /// <param name="newContactPhone">New contact phone string, or null/empty to clear.</param>
    public void UpdateContactPhone(string? newContactPhone)
    {
        this.ContactPhone = string.IsNullOrWhiteSpace(newContactPhone)
            ? null
            : PhoneNumber.Create(newContactPhone);

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the title of the aid request.
    /// Regenerates the slug.
    /// </summary>
    /// <param name="newTitle">New title value.</param>
    /// <exception cref="ArgumentException">Thrown when the title is invalid.</exception>
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Назва не може бути порожньою.", nameof(newTitle));
        }

        this.Title = Title.Create(newTitle);
        this.RegenerateSlug();
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the description of the aid request.
    /// </summary>
    /// <param name="newDescription">New description value, can be null.</param>
    public void UpdateDescription(string? newDescription)
    {
        this.Description = newDescription;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the category of the aid request.
    /// </summary>
    /// <param name="newCategory">New category value.</param>
    public void UpdateCategory(AidCategory newCategory)
    {
        this.Category = newCategory;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Regenerates slug from current title.
    /// </summary>
    public void RegenerateSlug()
    {
        this.Slug = Slug.Create(this.Title.Value);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets urgency flag.
    /// </summary>
    /// <param name="isUrgent">Whether the request is urgent.</param>
    public void SetUrgency(bool isUrgent)
    {
        this.IsUrgent = isUrgent;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Registers a donation for this aid request. This method expects a fully-created Donation entity.
    /// It will create the linking entity, add it to the collection, update collected amount and change status if needed.
    /// </summary>
    /// <param name="donation">The <see cref="Donation"/> that was created/processed and should be linked.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="donation"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the request is cancelled or donation amount is invalid.</exception>
    public void RegisterDonation(Donation donation)
    {
        if (donation == null)
        {
            throw new ArgumentNullException(nameof(donation), "Донація не може бути null.");
        }

        if (this.Status == AidStatus.Cancelled)
        {
            throw new InvalidOperationException("Не можна додати пожертву до скасованого запиту.");
        }

        if (donation.Amount <= 0)
        {
            throw new InvalidOperationException("Сума пожертви має бути більшою за нуль.");
        }

        // prevent duplicate linking by DonationId
        if (this.donations.Any(d => d.DonationId == donation.Id))
        {
            // already linked — ignore or throw; here we ignore to be idempotent
            return;
        }

        var link = AnimalAidDonation.Create(donation.Id, this.Id);
        this.donations.Add(link);

        // update collected amount from the Donation entity
        this.collectedAmount += donation.Amount;

        // optionally close the request if collected enough
        if (this.EstimatedCost.HasValue && this.collectedAmount >= this.EstimatedCost.Value)
        {
            this.Status = AidStatus.Fulfilled;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Attaches a donation to this AnimalAidRequest by DonationId.
    /// </summary>
    /// <param name="donationId">ID of the Donation to attach.</param>
    public void AttachDonation(Guid donationId)
    {
        // Prevent duplicate
        if (this.donations.Any(d => d.DonationId == donationId))
        {
            return;
        }

        var link = AnimalAidDonation.Create(donationId, this.Id);
        this.donations.Add(link);

        // Note: CollectedAmount is updated when actual Donation entity is linked via RegisterDonation
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a photo URL to the shelter.
    /// </summary>
    /// <param name="photoUrl">The photo URL to add.</param>
    public void AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new ArgumentException("URL фото не може бути порожнім.", nameof(photoUrl));
        }

        this.photos.Add(photoUrl);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ShelterPhotoAddedEvent(this.Id, photoUrl));
    }

    /// <summary>
    /// Removes a photo URL from the shelter.
    /// </summary>
    /// <param name="photoUrl">The photo URL to remove.</param>
    /// <returns>True if removed; otherwise, false.</returns>
    public bool RemovePhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            return false;
        }

        var removed = this.photos.Remove(photoUrl);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new ShelterPhotoRemovedEvent(this.Id, photoUrl));
        }

        return removed;
    }

    /// <summary>
    /// Removes a donation link from this AnimalAidRequest.
    /// Note: this does not revert Donation.Amount — caller must decide how to handle Donation entity.
    /// </summary>
    /// <param name="donationId">The donation id to unlink.</param>
    /// <exception cref="InvalidOperationException">Thrown if the link is not found.</exception>
    public void RemoveDonationLink(Guid donationId)
    {
        var link = this.donations.FirstOrDefault(d => d.DonationId == donationId);
        if (link == null)
        {
            throw new InvalidOperationException("Зв'язок пожертви не знайдено у запиті.");
        }

        this.donations.Remove(link);

        // adjust collected amount only if Donation entity is available via navigation
        if (link.Donation != null)
        {
            this.collectedAmount -= link.Donation.Amount;
            if (this.collectedAmount < 0)
            {
                this.collectedAmount = 0;
            }
        }

        this.UpdatedAt = DateTime.UtcNow;
    }
}
