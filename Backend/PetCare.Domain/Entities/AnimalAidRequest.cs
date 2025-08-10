namespace PetCare.Domain.Entities;

using PetCare.Domain.Abstractions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a request for aid related to animals in the system.
/// </summary>
public sealed class AnimalAidRequest : BaseEntity
{
    private readonly List<string> photos = new();
    private decimal collectedAmount;

    private AnimalAidRequest()
    {
        this.Title = Title.Create(string.Empty);
    }

    private AnimalAidRequest(
        Guid? userId,
        Guid? shelterId,
        Title title,
        string? description,
        AidCategory category,
        AidStatus status,
        decimal? estimatedCost,
        List<string>? photos)
    {
        if (estimatedCost is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedCost), "Орієнтовна вартість має бути невід'ємною");
        }

        this.UserId = userId;
        this.ShelterId = shelterId;
        this.Title = title;
        this.Description = description;
        this.Category = category;
        this.Status = status;
        this.EstimatedCost = estimatedCost;
        this.photos = photos ?? new List<string>();
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the title of the aid request.
    /// </summary>
    public Title Title { get; private set; }

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
    /// <param name="description">The description of the aid request, if any. Can be null.</param>
    /// <param name="category">The category of the aid request.</param>
    /// <param name="status">The current status of the aid request.</param>
    /// <param name="estimatedCost">The estimated cost of the aid request, if known. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the aid request. Can be null.</param>
    /// <returns>A new instance of <see cref="AnimalAidRequest"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="estimatedCost"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is invalid according to <see cref="Title.Create"/>.</exception>
    public static AnimalAidRequest Create(
        Guid? userId,
        Guid? shelterId,
        string title,
        string? description,
        AidCategory category,
        AidStatus status,
        decimal? estimatedCost,
        List<string>? photos)
    {
        return new AnimalAidRequest(
            userId,
            shelterId,
            Title.Create(title),
            description,
            category,
            status,
            estimatedCost,
            photos);
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
    /// Adds a donation to the aid request.
    /// </summary>
    /// <param name="amount">The donation amount.</param>
    /// <exception cref="InvalidOperationException">Thrown if the aid request is closed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the donation amount is less than or equal to zero.</exception>
    public void AddDonation(decimal amount)
    {
        if (this.Status == AidStatus.Cancelled)
        {
            throw new InvalidOperationException("Збір вже завершено, пожертви не приймаються.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Сума пожертви має бути більшою за нуль.");
        }

        this.collectedAmount += amount;

        // Optionally close the request if collected enough
        if (this.EstimatedCost.HasValue && this.collectedAmount >= this.EstimatedCost.Value)
        {
            this.Status = AidStatus.Cancelled;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Asynchronously adds a photo to the shelter with validation and uploads it via the file storage service.
    /// </summary>
    /// <param name="fileStorage">The file storage service.</param>
    /// <param name="fileStream">The stream of the file to upload.</param>
    /// <param name="fileName">The name of the file for validation and storage.</param>
    /// <param name="fileSizeBytes">The size of the file in bytes for validation.</param>
    /// <param name="config">The media validation configuration (size and extensions).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public async Task AddPhotoAsync(
        IFileStorageService fileStorage,
        Stream fileStream,
        string fileName,
        long fileSizeBytes,
        MediaConfig config)
    {
        if (fileStream == null)
        {
            throw new ArgumentNullException(nameof(fileStream));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Ім'я файлу не може бути порожнім.", nameof(fileName));
        }

        config.Validate(fileName, fileSizeBytes);

        var photoUrl = await fileStorage.UploadAsync(fileStream, fileName, config.maxSizeBytes, config.allowedExtensions);
        this.photos.Add(photoUrl);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Asynchronously removes a photo from the shelter and deletes the file via the file storage service.
    /// </summary>
    /// <param name="fileStorage">The file storage service.</param>
    /// <param name="photoUrl">The URL of the photo to remove.</param>
    /// <returns>A task with a result indicating whether the photo was successfully removed.</returns>
    public async Task<bool> RemovePhotoAsync(IFileStorageService fileStorage, string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            return false;
        }

        var removed = this.photos.Remove(photoUrl);
        if (removed)
        {
            await fileStorage.DeleteAsync(photoUrl);
            this.UpdatedAt = DateTime.UtcNow;
        }

        return removed;
    }
}
