namespace PetCare.Domain.Aggregates;

using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents an animal in the system.
/// </summary>
public sealed class Animal : AggregateRoot
{
    private readonly List<string> photos = new();
    private readonly List<string> videos = new();
    private readonly List<AdoptionApplication> adoptionApplications = new();
    private readonly List<Tag> tags = new();
    private readonly List<SuccessStory> successStories = new();
    private readonly List<AnimalSubscription> subscribers = new();
    private readonly List<string> specialNeeds = new();
    private readonly List<string> healthConditions = new();
    private readonly List<AnimalTemperament> temperaments = new();

    private Animal()
    {
        this.Slug = null!;
        this.Name = null!;
    }

    private Animal(
        Slug slug,
        Guid userId,
        Name name,
        Guid breedId,
        Birthday? birthday,
        AnimalGender gender,
        string? description,
        List<string> healthConditions,
        List<string> specialNeeds,
        List<AnimalTemperament> temperaments,
        AnimalSize size,
        List<string> photos,
        List<string>? videos,
        Guid shelterId,
        AnimalStatus status,
        AnimalCareCost careCost,
        string? adoptionRequirements,
        MicrochipId? microchipId,
        float? weight,
        float? height,
        string? color,
        bool isSterilized,
        bool isUnderCare,
        bool haveDocuments)
    {
        this.Slug = slug;
        this.UserId = userId != Guid.Empty
            ? userId
            : throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        this.Name = name;
        this.BreedId = breedId != Guid.Empty
           ? breedId
           : throw new ArgumentException("Ідентифікатор породи не може бути порожнім.", nameof(breedId));
        this.Birthday = birthday;
        this.Gender = gender;
        this.Description = description;
        this.healthConditions = healthConditions ?? new List<string>();
        this.specialNeeds = specialNeeds ?? new List<string>();
        this.temperaments = temperaments ?? new List<AnimalTemperament>();
        this.Size = size;
        this.photos = photos ?? new List<string>();
        this.videos = videos ?? new List<string>();
        this.ShelterId = shelterId != Guid.Empty
           ? shelterId
           : throw new ArgumentException("Ідентифікатор притулку не може бути порожнім.", nameof(shelterId));
        this.Status = status;
        this.CareCost = careCost;
        this.AdoptionRequirements = adoptionRequirements;
        this.MicrochipId = microchipId;
        this.Weight = weight;
        this.Height = height;
        this.Color = color;
        this.IsSterilized = isSterilized;
        this.IsUnderCare = isUnderCare;
        this.HaveDocuments = haveDocuments;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the animal.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the animal.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the birthday of the animal, if known. Can be null.
    /// </summary>
    public Birthday? Birthday { get; private set; }

    /// <summary>
    /// Gets the gender of the animal.
    /// </summary>
    public AnimalGender Gender { get; private set; }

    /// <summary>
    /// Gets the description of the animal, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the health conditions of the animal.
    /// </summary>
    public IReadOnlyCollection<string> HealthConditions => this.healthConditions.AsReadOnly();

    /// <summary>
    /// Gets the special needs of the animal.
    /// </summary>
    public IReadOnlyCollection<string> SpecialNeeds => this.specialNeeds.AsReadOnly();

    /// <summary>
    /// Gets the size of the animal.
    /// </summary>
    public AnimalSize Size { get; private set; }

    /// <summary>
    /// Gets the temperaments of the animal.
    /// </summary>
    public IReadOnlyCollection<AnimalTemperament> Temperaments => this.temperaments.AsReadOnly();

    /// <summary>
    /// Gets the photos of the animal.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the videos of the animal.
    /// </summary>
    public IReadOnlyList<string> Videos => this.videos.AsReadOnly();

    /// <summary>
    /// Gets the adoption applications associated with this animal.
    /// </summary>
    public IReadOnlyList<AdoptionApplication> AdoptionApplications =>
        this.adoptionApplications.AsReadOnly();

    /// <summary>
    /// Gets the list of tags associated with the animal.
    /// </summary>
    public IReadOnlyCollection<Tag> Tags => this.tags.AsReadOnly();

    /// <summary>
    /// Gets the current status of the animal.
    /// </summary>
    public AnimalStatus Status { get; private set; }

    /// <summary>
    /// Gets the care cost of the animal.
    /// </summary>
    public AnimalCareCost CareCost { get; private set; }

    /// <summary>
    /// Gets the adoption requirements for the animal, if any. Can be null.
    /// </summary>
    public string? AdoptionRequirements { get; private set; }

    /// <summary>
    /// Gets the microchip identifier for the animal, if any. Can be null.
    /// </summary>
    public MicrochipId? MicrochipId { get; private set; }

    /// <summary>
    /// Gets the weight of the animal in kilograms, if known. Can be null.
    /// </summary>
    public float? Weight { get; private set; }

    /// <summary>
    /// Gets the height of the animal in centimeters, if known. Can be null.
    /// </summary>
    public float? Height { get; private set; }

    /// <summary>
    /// Gets the color of the animal, if any. Can be null.
    /// </summary>
    public string? Color { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal is sterilized.
    /// </summary>
    public bool IsSterilized { get; private set; }

    /// <summary>
    /// Gets a value indicating whether indicates whether the animal is currently under care.
    /// </summary>
    public bool IsUnderCare { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal has documents.
    /// </summary>
    public bool HaveDocuments { get; private set; }

    /// <summary>
    /// Gets the date and time when the animal record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the animal record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user associated with the animal.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the user associated with the animal, if any. Can be null.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the animal's breed.
    /// </summary>
    public Guid BreedId { get; private set; }

    /// <summary>
    /// Gets the breed of the animal, if any. Can be null.
    /// </summary>
    public Breed? Breed { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter hosting the animal.
    /// </summary>
    public Guid ShelterId { get; private set; }

    /// <summary>
    /// Gets the shelter hosting the animal, if any. Can be null.
    /// </summary>
    public Shelter? Shelter { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animal is eligible for adoption.
    /// </summary>
    public bool CanBeAdopted => this.Status == AnimalStatus.Available;

    /// <summary>
    /// Gets a value indicating whether the animal has a microchip.
    /// </summary>
    public bool HasMicrochip => this.MicrochipId is not null;

    /// <summary>
    /// Gets the success stories related to this animal.
    /// </summary>
    public IReadOnlyCollection<SuccessStory> SuccessStories => this.successStories.AsReadOnly();

    /// <summary>
    /// Gets the subscribers of the animal.
    /// </summary>
    public IReadOnlyCollection<AnimalSubscription> Subscribers => this.subscribers.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Animal"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the animal.</param>
    /// <param name="name">The name of the animal.</param>
    /// <param name="breedId">The unique identifier of the animal's breed.</param>
    /// <param name="birthday">The birthday of the animal, if known. Can be null.</param>
    /// <param name="gender">The gender of the animal.</param>
    /// <param name="description">The description of the animal, if any. Can be null.</param>
    /// <param name="healthConditions">The list of health conditions for the animal. Can be null.</param>
    /// <param name="specialNeeds">The list of special needs for the animal. Can be null.</param>
    /// <param name="temperaments">The list of temperaments for the animal. Can be null.</param>
    /// <param name="size">The size of the animal.</param>
    /// <param name="photos">The list of photo URLs for the animal. Can be null.</param>
    /// <param name="videos">The list of video URLs for the animal. Can be null.</param>
    /// <param name="shelterId">The unique identifier of the shelter hosting the animal.</param>
    /// <param name="status">The current status of the animal.</param>
    /// <param name="careCost">The care cost of the animal.</param>
    /// <param name="adoptionRequirements">The adoption requirements for the animal, if any. Can be null.</param>
    /// <param name="microchipId">The microchip identifier for the animal, if any. Can be null.</param>
    /// <param name="weight">The weight of the animal in kilograms, if known. Can be null.</param>
    /// <param name="height">The height of the animal in centimeters, if known. Can be null.</param>
    /// <param name="color">The color of the animal, if any. Can be null.</param>
    /// <param name="isSterilized">Indicates whether the animal is sterilized.</param>
    /// <param name="isUnderCare">Indicates whether the animal is under care.</param>
    /// <param name="haveDocuments">Indicates whether the animal has documents.</param>
    /// <returns>A new instance of <see cref="Animal"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static Animal Create(
        Guid userId,
        string name,
        Guid breedId,
        Birthday? birthday,
        AnimalGender gender,
        string? description,
        List<string>? healthConditions,
        List<string>? specialNeeds,
        List<AnimalTemperament>? temperaments,
        AnimalSize size,
        List<string>? photos,
        List<string>? videos,
        Guid shelterId,
        AnimalStatus status,
        AnimalCareCost careCost,
        string? adoptionRequirements,
        string? microchipId,
        float? weight,
        float? height,
        string? color,
        bool isSterilized = false,
        bool isUnderCare = false,
        bool haveDocuments = false)
    {
        if (isUnderCare && status is not (AnimalStatus.Available or AnimalStatus.Reserved or AnimalStatus.InTreatment))
        {
            throw new InvalidOperationException(
                "Тварину можна створити під опікою лише якщо вона перебуває в притулку або на лікуванні.");
        }

        var animal = new Animal(
            Slug.Create(name),
            userId,
            Name.Create(name),
            breedId,
            birthday is not null ? Birthday.Create(birthday.Value) : null,
            gender,
            description,
            healthConditions ?? new(),
            specialNeeds ?? new(),
            temperaments ?? new(),
            size,
            photos ?? new(),
            videos ?? new(),
            shelterId,
            status,
            careCost,
            adoptionRequirements,
            microchipId is not null ? MicrochipId.Create(microchipId) : null,
            weight,
            height,
            color,
            isSterilized,
            isUnderCare,
            haveDocuments);
        animal.AddDomainEvent(new AnimalCreatedEvent(animal.Id, animal.Slug, animal.Name));
        return animal;
    }

    /// <summary>
    /// Updates the properties of the animal, if provided.
    /// </summary>
    /// <param name="name">The new name of the animal, if provided. If null, the name remains unchanged.</param>
    /// <param name="birthday">The new birthday of the animal, if provided. If null, the birthday remains unchanged.</param>
    /// <param name="gender">The new gender of the animal, if provided. If null, the gender remains unchanged.</param>
    /// <param name="description">The new description of the animal, if provided. If null, the description remains unchanged.</param>
    /// <param name="status">The new status of the animal, if provided. If null, the status remains unchanged.</param>
    /// <param name="adoptionRequirements">The new adoption requirements for the animal, if provided. If null, the adoption requirements remain unchanged.</param>
    /// <param name="microchipId">The new microchip identifier for the animal, if provided. If null, the microchip identifier remains unchanged.</param>
    /// <param name="weight">The new weight of the animal in kilograms, if provided. If null, the weight remains unchanged.</param>
    /// <param name="height">The new height of the animal in centimeters, if provided. If null, the height remains unchanged.</param>
    /// <param name="color">The new color of the animal, if provided. If null, the color remains unchanged.</param>
    /// <param name="isSterilized">Indicates whether the animal is sterilized, if provided. If null, the sterilization status remains unchanged.</param>
    /// <param name="isUnderCare">Indicates whether the animal is under care.</param>
    /// <param name="haveDocuments">Indicates whether the animal has documents, if provided. If null, the document status remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> or <paramref name="microchipId"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public void Update(
        string? name = null,
        Birthday? birthday = null,
        AnimalGender? gender = null,
        string? description = null,
        AnimalStatus? status = null,
        string? adoptionRequirements = null,
        string? microchipId = null,
        float? weight = null,
        float? height = null,
        string? color = null,
        bool? isSterilized = null,
        bool? isUnderCare = null,
        bool? haveDocuments = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = Name.Create(name);
            this.Slug = Slug.Create(name);
        }

        if (birthday is not null)
        {
            this.Birthday = birthday;
        }

        if (gender is not null)
        {
            this.Gender = gender.Value;
        }

        if (description is not null)
        {
            this.Description = description;
        }

        if (status is not null)
        {
            this.Status = status.Value;

            if (this.IsUnderCare &&
                this.Status is not (AnimalStatus.Available or AnimalStatus.Reserved or AnimalStatus.InTreatment))
            {
                throw new InvalidOperationException(
                    "Тварина не може залишатися під опікою, якщо вона не перебуває в притулку або на лікуванні.");
            }
        }

        if (adoptionRequirements is not null)
        {
            this.AdoptionRequirements = adoptionRequirements;
        }

        if (microchipId is not null)
        {
            this.MicrochipId = MicrochipId.Create(microchipId);
        }

        if (weight is not null)
        {
            this.Weight = weight;
        }

        if (height is not null)
        {
            this.Height = height;
        }

        if (color is not null)
        {
            this.Color = color;
        }

        if (isSterilized is not null)
        {
            this.IsSterilized = isSterilized.Value;
        }

        if (isUnderCare is not null)
        {
            if (isUnderCare.Value &&
                this.Status is not (AnimalStatus.Available or AnimalStatus.Reserved or AnimalStatus.InTreatment))
            {
                throw new InvalidOperationException(
                    "Тварину можна позначити як під опікою лише якщо вона перебуває в притулку або на лікуванні.");
            }

            this.IsUnderCare = isUnderCare.Value;
        }

        if (haveDocuments is not null)
        {
            this.HaveDocuments = haveDocuments.Value;
        }

        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalUpdatedEvent(this.Id));
    }

    /// <summary>
    /// Updates the health conditions of the animal.
    /// </summary>
    /// <param name="conditions">The new list of health conditions. If null, the list is cleared.</param>
    public void UpdateHealthConditions(List<string> conditions)
    {
        this.healthConditions.Clear();
        this.healthConditions.AddRange(conditions ?? new List<string>());
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the special needs of the animal.
    /// </summary>
    /// <param name="needs">The new list of special needs. If null, the list is cleared.</param>
    public void UpdateSpecialNeeds(List<string> needs)
    {
        this.specialNeeds.Clear();
        this.specialNeeds.AddRange(needs ?? new List<string>());
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the temperaments of the animal.
    /// </summary>
    /// <param name="values">The new list of temperaments. If null, the list is cleared.</param>
    public void UpdateTemperaments(List<AnimalTemperament> values)
    {
        this.temperaments.Clear();
        this.temperaments.AddRange(values ?? new List<AnimalTemperament>());
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the size of the animal and sets the update timestamp.
    /// </summary>
    /// <param name="newSize">The new size to apply.</param>
    public void UpdateSize(AnimalSize newSize)
    {
        this.Size = newSize;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the care cost of the animal.
    /// </summary>
    /// <param name="newCost">The new care cost.</param>
    public void UpdateCareCost(AnimalCareCost newCost)
    {
        this.CareCost = newCost;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Changes the status of the animal.
    /// </summary>
    /// /// <param name="newStatus">The new status to apply.</param>
    public void ChangeStatus(AnimalStatus newStatus)
    {
        this.Status = newStatus;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalStatusChangedEvent(this.Id, newStatus));
    }

    /// <summary>
    /// Adds a photo URL to the success story.
    /// </summary>
    /// <param name="photoUrl">The photo URL to add.</param>
    /// <exception cref="ArgumentException">Thrown when photoUrl is invalid.</exception>
    public void AddPhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new ArgumentException("URL фото не може бути порожнім.");
        }

        this.photos.Add(photoUrl);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalPhotoAddedEvent(this.Id, photoUrl));
    }

    /// <summary>
    /// Removes a photo URL from the success story.
    /// </summary>
    /// <param name="photoUrl">The photo URL to remove.</param>
    /// <returns><c>true</c> if photo was removed; otherwise, <c>false</c>.</returns>
    public bool RemovePhoto(string photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            throw new InvalidOperationException("URL фото не може бути порожнім.");
        }

        var normalizedUrl = photoUrl.Trim().ToLowerInvariant();

        var existingPhoto = this.photos.FirstOrDefault(p =>
        p.Trim().ToLowerInvariant() == normalizedUrl);

        if (existingPhoto is null)
        {
            throw new InvalidOperationException($"Фото з URL '{photoUrl}' не знайдено.");
        }

        var removed = this.photos.Remove(existingPhoto);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new AnimalPhotoRemovedEvent(this.Id, photoUrl));
        }

        return removed;
    }

    /// <summary>
    /// Adds a video URL to the success story.
    /// </summary>
    /// <param name="videoUrl">The video URL to add.</param>
    /// <exception cref="ArgumentException">Thrown when videoUrl is invalid.</exception>
    public void AddVideo(string videoUrl)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            throw new ArgumentException("URL відео не може бути порожнім.");
        }

        this.videos.Add(videoUrl);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalVideoAddedEvent(this.Id, videoUrl));
    }

    /// <summary>
    /// Removes a video URL from the success story.
    /// </summary>
    /// <param name="videoUrl">The video URL to remove.</param>
    /// <returns><c>true</c> if video was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveVideo(string videoUrl)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            return false;
        }

        var removed = this.videos.Remove(videoUrl);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new AnimalVideoRemovedEvent(this.Id, videoUrl));
        }

        return removed;
    }

    /// <summary>
    /// Validates whether the animal has sufficient adoption requirements defined.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the animal's adoption requirements are missing or too short.
    /// </exception>
    public void ValidateAdoptionRequirements()
    {
        if (string.IsNullOrWhiteSpace(this.AdoptionRequirements) || this.AdoptionRequirements.Length < 10)
        {
            throw new InvalidOperationException("Вимоги до адопції тварини не заповнені або занадто короткі.");
        }
    }

    // AdoptionApplication

    /// <summary>
    /// Adds a new adoption application for the animal.
    /// </summary>
    /// <param name="application">The adoption application to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must be the owner of the application or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown if application is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the application already exists.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the requesting user is not authorized to add the application.</exception>
    public void AddAdoptionApplication(AdoptionApplication application, Guid requestingUserId)
    {
        if (application is null)
        {
            throw new ArgumentNullException(nameof(application), "Заявка на усиновлення не може бути null.");
        }

        if (this.adoptionApplications.Any(a => a.Id == application.Id))
        {
            throw new InvalidOperationException("Ця заявка вже додана для цієї тварини.");
        }

        if (!this.CanManageAdoptionApplications(requestingUserId))
        {
            throw new UnauthorizedAccessException("Тільки власник заявки або адміністратор/модератор може додавати заявку.");
        }

        this.adoptionApplications.Add(application);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AdoptionApplicationAddedEvent(this.Id, application.Id));
    }

    /// <summary>
    /// Removes an adoption application from the animal.
    /// </summary>
    /// <param name="applicationId">The ID of the adoption application to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown if the application is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the requesting user is not authorized to remove the application.</exception>
    public void RemoveAdoptionApplication(Guid applicationId, Guid requestingUserId)
    {
        var application = this.adoptionApplications.FirstOrDefault(a => a.Id == applicationId);
        if (application == null)
        {
            throw new InvalidOperationException("Заявка на усиновлення не знайдена.");
        }

        if (!this.CanManageAdoptionApplications(requestingUserId))
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення цієї заявки.");
        }

        this.adoptionApplications.Remove(application);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AdoptionApplicationRemovedEvent(this.Id, applicationId));
    }

    // Tag

    /// <summary>
    /// Adds a new tag to the animal.
    /// </summary>
    /// <param name="tag">The tag entity to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the tag is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the tag already exists for this animal.</exception>
    public void AddTag(Tag tag)
    {
        if (tag is null)
        {
            throw new ArgumentNullException(nameof(tag), "Тег не може бути null.");
        }

        if (this.tags.Any(t => t.Id == tag.Id))
        {
            throw new InvalidOperationException($"Тег '{tag.Name}' вже додано до тварини.");
        }

        this.tags.Add(tag);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an existing tag from the animal.
    /// </summary>
    /// <param name="tagId">The ID of the tag to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown when the tag is not found for this animal.</exception>
    public void RemoveTag(Guid tagId)
    {
        var tag = this.tags.FirstOrDefault(t => t.Id == tagId);

        if (tag is null)
        {
            throw new InvalidOperationException($"Тег з ID '{tagId}' не знайдено у тварини.");
        }

        this.tags.Remove(tag);
        this.UpdatedAt = DateTime.UtcNow;
    }

    // SuccessStory

    /// <summary>
    /// Adds a new success story for this animal.
    /// </summary>
    /// <param name="story">The success story to add.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <exception cref="InvalidOperationException">Thrown when the user is not allowed to add a story.</exception>
    public void AddSuccessStory(SuccessStory story, Guid userId)
    {
        if (!this.CanManageAdoptionApplications(userId))
        {
            throw new InvalidOperationException("Ви не маєте права додавати історію успіху для цієї тварини.");
        }

        if (story == null)
        {
            throw new ArgumentNullException(nameof(story), "Історія успіху не може бути null.");
        }

        this.successStories.Add(story);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new SuccessStoryAddedEvent(this.Id, story.Id));
    }

    /// <summary>
    /// Removes a success story from this animal.
    /// </summary>
    /// <param name="storyId">The ID of the story to remove.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <exception cref="InvalidOperationException">Thrown when the user is not allowed to remove the story.</exception>
    public void RemoveSuccessStory(Guid storyId, Guid userId)
    {
        if (!this.CanManageAdoptionApplications(userId))
        {
            throw new InvalidOperationException("Ви не маєте права видаляти історію успіху для цієї тварини.");
        }

        var story = this.successStories.FirstOrDefault(s => s.Id == storyId);
        if (story == null)
        {
            throw new InvalidOperationException("Історія успіху не знайдена.");
        }

        this.successStories.Remove(story);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new SuccessStoryRemovedEvent(this.Id, storyId));
    }

    /// <summary>
    /// Subscribes a user to the animal if not already subscribed.
    /// </summary>
    /// <param name="userId">The ID of the user subscribing to the animal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the user is already subscribed.</exception>
    /// <returns>The created <see cref="AnimalSubscription"/> instance.</returns>
    public AnimalSubscription SubscribeUser(Guid userId)
    {
        if (this.subscribers.Any(s => s.UserId == userId))
        {
            throw new InvalidOperationException("Користувач вже підписаний на тварину.");
        }

        var subscription = AnimalSubscription.Create(userId, this.Id);
        this.subscribers.Add(subscription);

        this.AddDomainEvent(new UserSubscribedToAnimalEvent(this.Id, userId));

        return subscription;
    }

    /// <summary>
    /// Unsubscribes a user from the animal if subscribed.
    /// </summary>
    /// <param name="userId">The ID of the user unsubscribing from the animal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the user is not subscribed.</exception>
    /// <returns>The removed <see cref="AnimalSubscription"/> instance.</returns>
    public AnimalSubscription UnsubscribeUser(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        var subscription = this.subscribers.FirstOrDefault(s => s.UserId == userId);

        if (subscription is null)
        {
            throw new InvalidOperationException("Користувач не підписаний на цю тварину.");
        }

        this.subscribers.Remove(subscription);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new UserUnsubscribedFromAnimalEvent(this.Id, userId));

        return subscription;
    }

    /// <summary>
    /// Checks if the specified user is subscribed to the animal.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns><c>true</c> if the user is subscribed; otherwise, <c>false</c>.</returns>
    public bool IsSubscribed(Guid userId)
        => this.subscribers.Any(s => s.UserId == userId);

    /// <summary>
    /// Checks if the given user is the owner of the animal.
    /// </summary>
    /// <param name="userId">The ID of the user to check.</param>
    /// <returns>True if the user is the owner of the animal; otherwise, false.</returns>
    public bool IsOwner(Guid userId) => this.UserId == userId;

    /// <summary>
    /// Checks if the given user can manage adoption applications (owner or admin/moderator).
    /// </summary>
    /// <param name="userId">The ID of the user to check.</param>
    /// <returns>True if the user is the owner or has Admin/Moderator role; otherwise, false.</returns>
    public bool CanManageAdoptionApplications(Guid userId) =>
        this.IsOwner(userId) || (this.User != null && (this.User.Role == UserRole.Admin || this.User.Role == UserRole.Moderator));

    /// <summary>Marks animal as being under guardianship.</summary>
    public void MarkAsUnderCare()
    {
        if (!this.IsUnderCare)
        {
            this.IsUnderCare = true;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>Removes guardianship mark from animal.</summary>
    public void MarkAsNotUnderCare()
    {
        if (this.IsUnderCare)
        {
            this.IsUnderCare = false;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}
