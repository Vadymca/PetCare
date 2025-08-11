namespace PetCare.Domain.Entities;
using PetCare.Domain.Abstractions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a lost pet record in the system.
/// </summary>
public sealed class LostPet : BaseEntity
{
    private readonly List<string> photos = new();

    private LostPet()
    {
        this.Slug = Slug.Create(string.Empty);
    }

    private LostPet(
        Slug slug,
        Guid userId,
        Guid? breedId,
        Name? name,
        string? description,
        Coordinates? lastSeenLocation,
        DateTime? lastSeenDate,
        IEnumerable<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        decimal? reward,
        string? contactAlternative,
        MicrochipId? microchipId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        this.Slug = slug;
        this.UserId = userId;
        this.BreedId = breedId;
        this.Name = name;
        this.Description = description;
        this.LastSeenLocation = lastSeenLocation;
        this.LastSeenDate = lastSeenDate;
        if (photos != null)
        {
            this.photos.AddRange(photos);
        }

        if (reward is < 0)
        {
            throw new ArgumentException("Розмір винагороди не може бути від’ємним.", nameof(reward));
        }

        this.Status = status;
        this.AdminNotes = adminNotes;
        this.Reward = reward;
        this.ContactAlternative = contactAlternative;
        this.MicrochipId = microchipId;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the lost pet record.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the lost pet, if any. Can be null.
    /// </summary>
    public Name? Name { get; private set; }

    /// <summary>
    /// Gets the description of the lost pet, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the last known geographic location of the pet, if any. Can be null.
    /// </summary>
    public Coordinates? LastSeenLocation { get; private set; }

    /// <summary>
    /// Gets the date and time when the pet was last seen, if any. Can be null.
    /// </summary>
    public DateTime? LastSeenDate { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the lost pet.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the current status of the lost pet record.
    /// </summary>
    public LostPetStatus Status { get; private set; }

    /// <summary>
    /// Gets the administrative notes for the lost pet record, if any. Can be null.
    /// </summary>
    public string? AdminNotes { get; private set; }

    /// <summary>
    /// Gets the reward offered for finding the pet, if any. Can be null.
    /// </summary>
    public decimal? Reward { get; private set; }

    /// <summary>
    /// Gets the alternative contact information for the lost pet, if any. Can be null.
    /// </summary>
    public string? ContactAlternative { get; private set; }

    /// <summary>
    /// Gets the microchip identifier of the pet, if any. Can be null.
    /// </summary>
    public MicrochipId? MicrochipId { get; private set; }

    /// <summary>
    /// Gets the date and time when the lost pet record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the lost pet record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user reporting the lost pet.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the user who reported the lost pet.
    /// Navigation property for EF Core.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the pet's breed, if any. Can be null.
    /// </summary>
    public Guid? BreedId { get; private set; }

    /// <summary>
    /// Gets the breed of the lost pet.
    /// Navigation property for EF Core.
    /// </summary>
    public Breed? Breed { get; private set; }

    /// <summary>
    /// Creates a new <see cref="LostPet"/> instance with the specified parameters.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the lost pet record.</param>
    /// <param name="userId">The unique identifier of the user reporting the lost pet.</param>
    /// <param name="breedId">The unique identifier of the pet's breed, if any. Can be null.</param>
    /// <param name="name">The name of the lost pet, if any. Can be null or empty.</param>
    /// <param name="description">The description of the lost pet, if any. Can be null.</param>
    /// <param name="lastSeenLocation">The last known geographic location of the pet, if any. Can be null.</param>
    /// <param name="lastSeenDate">The date and time when the pet was last seen, if any. Can be null.</param>
    /// <param name="photos">The list of photo URLs for the lost pet, if any. Can be null.</param>
    /// <param name="status">The current status of the lost pet record.</param>
    /// <param name="adminNotes">The administrative notes for the lost pet record, if any. Can be null.</param>
    /// <param name="reward">The reward offered for finding the pet, if any. Can be null.</param>
    /// <param name="contactAlternative">The alternative contact information for the lost pet, if any. Can be null.</param>
    /// <param name="microchipId">The microchip identifier of the pet, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="LostPet"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static LostPet Create(
        string slug,
        Guid userId,
        Guid? breedId,
        string? name,
        string? description,
        Coordinates? lastSeenLocation,
        DateTime? lastSeenDate,
        IEnumerable<string>? photos,
        LostPetStatus status,
        string? adminNotes,
        decimal? reward,
        string? contactAlternative,
        string? microchipId)
    {
        return new LostPet(
            Slug.Create(slug),
            userId,
            breedId,
            string.IsNullOrWhiteSpace(name) ? null : Name.Create(name),
            description,
            lastSeenLocation,
            lastSeenDate,
            photos,
            status,
            adminNotes,
            reward,
            contactAlternative,
            microchipId is not null ? MicrochipId.Create(microchipId) : null);
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

    /// <summary>
    /// Updates the name of the lost pet.
    /// </summary>
    /// <param name="newName">The new name. Null or whitespace will clear the current name.</param>
    public void UpdateName(string? newName)
    {
        this.Name = string.IsNullOrWhiteSpace(newName) ? null : Name.Create(newName);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the description of the lost pet.
    /// </summary>
    /// <param name="newDescription">The new description. Can be null.</param>
    public void UpdateDescription(string? newDescription)
    {
        this.Description = newDescription;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the last seen location and date.
    /// </summary>
    /// <param name="location">The new last seen location. Can be null.</param>
    /// <param name="date">The new last seen date. Can be null.</param>
    public void UpdateLastSeen(Coordinates? location, DateTime? date)
    {
        this.LastSeenLocation = location;
        this.LastSeenDate = date;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the alternative contact information.
    /// </summary>
    /// <param name="contact">The new alternative contact information. Can be null.</param>
    public void UpdateContactAlternative(string? contact)
    {
        this.ContactAlternative = contact;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the reward amount.
    /// </summary>
    /// <param name="rewardAmount">The new reward amount. Must not be negative.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="rewardAmount"/> is negative.</exception>
    public void UpdateReward(decimal? rewardAmount)
    {
        if (rewardAmount is < 0)
        {
            throw new ArgumentException("Розмір винагороди не може бути від’ємним.", nameof(rewardAmount));
        }

        this.Reward = rewardAmount;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the microchip ID.
    /// </summary>
    /// <param name="microchipId">The new microchip ID. Null to clear.</param>
    public void UpdateMicrochipId(string? microchipId)
    {
        this.MicrochipId = microchipId is null ? null : MicrochipId.Create(microchipId);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the lost pet's status, ensuring valid status transitions.
    /// </summary>
    /// <param name="newStatus">The new status to assign.</param>
    /// <exception cref="DomainException">
    /// Thrown when the status transition is not allowed.
    /// </exception>
    public void UpdateStatus(LostPetStatus newStatus)
    {
        if (!this.CanTransitionTo(newStatus))
        {
            throw new ArgumentException($"Неможливо змінити статус з {this.Status} на {newStatus}");
        }

        this.Status = newStatus;

        if (newStatus == LostPetStatus.Found)
        {
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Assigns or updates the microchip ID for the lost pet.
    /// </summary>
    /// <param name="microchipId">The microchip ID to assign.</param>
    /// <exception cref="InvalidOperationException">Thrown when the microchip ID is already assigned.</exception>
    public void SetMicrochip(string microchipId)
    {
        var newMicrochip = MicrochipId.Create(microchipId);

        if (this.MicrochipId is not null && this.MicrochipId.Equals(newMicrochip))
        {
            throw new InvalidOperationException("Цей мікрочіп вже призначено цьому запису.");
        }

        this.MicrochipId = newMicrochip;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifies if the provided microchip matches the stored microchip for this lost pet.
    /// </summary>
    /// <param name="chip">The microchip ID to verify.</param>
    /// <returns>
    /// <see langword="true"/> if the microchip matches; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="DomainException">
    /// Thrown when no microchip is registered for this record.
    /// </exception>
    public bool VerifyMicrochip(MicrochipId chip)
    {
        if (this.MicrochipId == null)
        {
            throw new ArgumentException("Мікрочіп не зареєстрований для цього запису");
        }

        return this.MicrochipId.Equals(chip);
    }

    /// <summary>
    /// Removes the microchip ID from the lost pet.
    /// </summary>
    public void ClearMicrochip()
    {
        this.MicrochipId = null;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Determines if the current status can transition to the specified new status.
    /// </summary>
    /// <param name="newStatus">The status to transition to.</param>
    /// <returns>
    /// <see langword="true"/> if the transition is valid; otherwise, <see langword="false"/>.
    /// </returns>
    private bool CanTransitionTo(LostPetStatus newStatus)
    {
        return (this.Status == LostPetStatus.Lost && newStatus == LostPetStatus.Found)
            || (this.Status == LostPetStatus.Found && newStatus == LostPetStatus.Reunited);
    }
}
