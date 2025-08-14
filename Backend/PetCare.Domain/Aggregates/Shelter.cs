namespace PetCare.Domain.Aggregates;

using PetCare.Domain.Abstractions;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a shelter in the system.
/// </summary>
public sealed class Shelter : AggregateRoot
{
    private readonly List<Animal> animals = new();
    private readonly List<string> photos = new();
    private readonly Dictionary<string, string> socialMedia = new();
    private readonly List<Donation> donations = new();
    private readonly List<VolunteerTask> volunteerTasks = new();
    private readonly List<AnimalAidRequest> animalAidRequests = new();
    private readonly List<IoTDevice> ioTDevices = new();
    private readonly List<Event> events = new();
    private readonly List<ShelterSubscription> subscribers = new();

    private Shelter()
    {
        this.Slug = Slug.Create(string.Empty);
        this.Name = Name.Create(string.Empty);
        this.Address = Address.Create(string.Empty);
        this.Coordinates = ValueObjects.Coordinates.Origin;
        this.ContactPhone = PhoneNumber.Create(string.Empty);
        this.ContactEmail = Email.Create("default@petcare.com");
    }

    private Shelter(
        Slug slug,
        Name name,
        Address address,
        ValueObjects.Coordinates coordinates,
        PhoneNumber contactPhone,
        Email contactEmail,
        string? description,
        int capacity,
        int currentOccupancy,
        List<string> photos,
        string? virtualTourUrl,
        string? workingHours,
        Dictionary<string, string> socialMedia,
        Guid managerId)
    {
        this.Slug = slug;
        this.Name = name;
        this.Address = address;
        this.Coordinates = coordinates;
        this.ContactPhone = contactPhone;
        this.ContactEmail = contactEmail;
        this.Description = description;
        this.Capacity = capacity;
        this.CurrentOccupancy = currentOccupancy;
        this.photos = photos;
        this.VirtualTourUrl = virtualTourUrl;
        this.WorkingHours = workingHours;
        this.socialMedia = socialMedia;
        this.ManagerId = managerId;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the unique slug identifier for the shelter.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Gets the name of the shelter.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Gets the address of the shelter.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Gets the geographic coordinates of the shelter.
    /// </summary>
    public ValueObjects.Coordinates Coordinates { get; private set; }

    /// <summary>
    /// Gets the contact phone number of the shelter.
    /// </summary>
    public PhoneNumber ContactPhone { get; private set; }

    /// <summary>
    /// Gets the contact email address of the shelter.
    /// </summary>
    public Email ContactEmail { get; private set; }

    /// <summary>
    /// Gets the description of the shelter, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the maximum capacity of the shelter.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Gets the current number of animals in the shelter.
    /// </summary>
    public int CurrentOccupancy { get; private set; }

    /// <summary>
    /// Gets the list of photo URLs for the shelter.
    /// </summary>
    public IReadOnlyList<string> Photos => this.photos.AsReadOnly();

    /// <summary>
    /// Gets the URL for the virtual tour of the shelter, if any. Can be null.
    /// </summary>
    public string? VirtualTourUrl { get; private set; }

    /// <summary>
    /// Gets the working hours of the shelter, if any. Can be null.
    /// </summary>
    public string? WorkingHours { get; private set; }

    /// <summary>
    /// Gets the social media links for the shelter.
    /// </summary>
    public IReadOnlyDictionary<string, string> SocialMedia => this.socialMedia;

    /// <summary>
    /// Gets the collection of donations made by the shelter.
    /// </summary>
    public IReadOnlyCollection<Donation> Donations => this.donations.AsReadOnly();

    /// <summary>
    /// Gets the collection of volunteer tasks for the shelter.
    /// </summary>
    public IReadOnlyCollection<VolunteerTask> VolunteerTasks => this.volunteerTasks.AsReadOnly();

    /// <summary>
    /// Gets the collection of animal aid requests associated with the shelter.
    /// </summary>
    public IReadOnlyCollection<AnimalAidRequest> AnimalAidRequests => this.animalAidRequests.AsReadOnly();

    /// <summary>
    /// Gets the list of IoT devices assigned to the shelter.
    /// </summary>
    public IReadOnlyCollection<IoTDevice> IoTDevices => this.ioTDevices.AsReadOnly();

    /// <summary>
    /// Gets the events organized by this shelter.
    /// </summary>
    public IReadOnlyCollection<Event> Events => this.events.AsReadOnly();

    /// <summary>
    /// Gets the subscribers by this shelter.
    /// </summary>
    public IReadOnlyCollection<ShelterSubscription> Subscribers => this.subscribers.AsReadOnly();

    /// <summary>
    /// Gets the date and time when the shelter record was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the shelter record was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the shelter's manager, if any. Can be null.
    /// </summary>
    public Guid? ManagerId { get; private set; }

    /// <summary>
    /// Gets the manager associated with the shelter, if any. Can be null.
    /// </summary>
    public User? Manager { get; private set; }

    /// <summary>
    /// Gets the identifiers of animals in the shelter.
    /// </summary>
    public IReadOnlyCollection<Animal> Animals => this.animals.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Shelter"/> instance with the specified parameters.
    /// </summary>
    /// <param name="slug">The unique slug identifier for the shelter.</param>
    /// <param name="name">The name of the shelter.</param>
    /// <param name="address">The address of the shelter.</param>
    /// <param name="coordinates">The geographic coordinates of the shelter.</param>
    /// <param name="contactPhone">The contact phone number of the shelter.</param>
    /// <param name="contactEmail">The contact email address of the shelter.</param>
    /// <param name="description">The description of the shelter, if any. Can be null.</param>
    /// <param name="capacity">The maximum capacity of the shelter.</param>
    /// <param name="currentOccupancy">The current number of animals in the shelter.</param>
    /// <param name="photos">The list of photo URLs for the shelter, if any. Can be null.</param>
    /// <param name="virtualTourUrl">The URL for the virtual tour of the shelter, if any. Can be null.</param>
    /// <param name="workingHours">The working hours of the shelter, if any. Can be null.</param>
    /// <param name="socialMedia">The social media links for the shelter, if any. Can be null.</param>
    /// <param name="managerId">The unique identifier of the shelter's manager, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="Shelter"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slug"/>, <paramref name="name"/>, <paramref name="address"/>, <paramref name="contactPhone"/>, or <paramref name="contactEmail"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static Shelter Create(
        string slug,
        string name,
        string address,
        ValueObjects.Coordinates coordinates,
        string contactPhone,
        string contactEmail,
        string? description,
        int capacity,
        int currentOccupancy,
        List<string>? photos,
        string? virtualTourUrl,
        string? workingHours,
        Dictionary<string, string>? socialMedia,
        Guid managerId)
    {
        var shelter = new Shelter(
            Slug.Create(slug),
            Name.Create(name),
            Address.Create(address),
            coordinates,
            PhoneNumber.Create(contactPhone),
            Email.Create(contactEmail),
            description,
            capacity,
            currentOccupancy,
            photos ?? new(),
            virtualTourUrl,
            workingHours,
            socialMedia ?? new(),
            managerId);

        shelter.AddDomainEvent(new ShelterCreatedEvent(shelter.Id));
        return shelter;
    }

    /// <summary>
    /// Updates the shelter's properties with the provided values.
    /// </summary>
    /// <param name="name">The new name of the shelter, if provided. If null or empty, the name remains unchanged.</param>
    /// <param name="address">The new address of the shelter, if provided. If null or empty, the address remains unchanged.</param>
    /// <param name="coordinates">The new geographic coordinates of the shelter, if provided. If null, the coordinates remain unchanged.</param>
    /// <param name="contactPhone">The new contact phone number of the shelter, if provided. If null or empty, the phone number remains unchanged.</param>
    /// <param name="contactEmail">The new contact email address of the shelter, if provided. If null or empty, the email remains unchanged.</param>
    /// <param name="description">The new description of the shelter, if provided. If null, the description remains unchanged.</param>
    /// <param name="capacity">The new maximum capacity of the shelter, if provided. If null, the capacity remains unchanged.</param>
    /// <param name="currentOccupancy">The new current number of animals in the shelter, if provided. If null, the occupancy remains unchanged.</param>
    /// <param name="photos">The new list of photo URLs for the shelter, if provided. If null, the photos remain unchanged.</param>
    /// <param name="virtualTourUrl">The new URL for the virtual tour of the shelter, if provided. If null, the virtual tour URL remains unchanged.</param>
    /// <param name="workingHours">The new working hours of the shelter, if provided. If null, the working hours remain unchanged.</param>
    /// <param name="socialMedia">The new social media links for the shelter, if provided. If null, the social media links remain unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/>, <paramref name="address"/>, <paramref name="contactPhone"/>, or <paramref name="contactEmail"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public void Update(
        string? name = null,
        string? address = null,
        ValueObjects.Coordinates? coordinates = null,
        string? contactPhone = null,
        string? contactEmail = null,
        string? description = null,
        int? capacity = null,
        int? currentOccupancy = null,
        List<string>? photos = null,
        string? virtualTourUrl = null,
        string? workingHours = null,
        Dictionary<string, string>? socialMedia = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = Name.Create(name);
        }

        if (!string.IsNullOrWhiteSpace(address))
        {
            this.Address = Address.Create(address);
        }

        if (coordinates != null)
        {
            this.Coordinates = coordinates;
        }

        if (!string.IsNullOrWhiteSpace(contactPhone))
        {
            this.ContactPhone = PhoneNumber.Create(contactPhone);
        }

        if (!string.IsNullOrWhiteSpace(contactEmail))
        {
            this.ContactEmail = Email.Create(contactEmail);
        }

        if (description != null)
        {
            this.Description = description;
        }

        if (capacity.HasValue)
        {
            this.Capacity = capacity.Value;
        }

        if (currentOccupancy.HasValue)
        {
            this.CurrentOccupancy = currentOccupancy.Value;
        }

        if (photos != null)
        {
            this.photos.Clear();
            this.photos.AddRange(photos);
        }

        if (virtualTourUrl != null)
        {
            this.VirtualTourUrl = virtualTourUrl;
        }

        if (workingHours != null)
        {
            this.WorkingHours = workingHours;
        }

        if (socialMedia != null)
        {
            this.socialMedia.Clear();
            foreach (var kvp in socialMedia)
            {
                this.socialMedia[kvp.Key] = kvp.Value;
            }
        }

        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ShelterUpdatedEvent(this.Id));
    }

    // Animals

    /// <summary>
    /// Adds a new animal to the shelter.
    /// </summary>
    /// <param name="animal">The animal to add.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <exception cref="InvalidOperationException">Thrown when the user does not have permission to add animals.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the animal is null.</exception>
    public void AddAnimal(Animal animal, Guid userId)
    {
        if (animal is null)
        {
            throw new ArgumentNullException(nameof(animal), "Тварина не може бути null.");
        }

        if (!this.IsManager(userId) && !this.IsAdminOrModerator())
        {
            throw new InvalidOperationException("Ви не маєте права додавати тварин до цього притулку.");
        }

        if (this.animals.Any(a => a.Id == animal.Id))
        {
            throw new InvalidOperationException("Ця тварина вже є у притулку.");
        }

        this.animals.Add(animal);
        this.CurrentOccupancy++;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalAddedToShelterEvent(this.Id, animal.Id, this.CurrentOccupancy));
    }

    /// <summary>
    /// Removes an animal from the shelter, updating the occupancy count.
    /// </summary>
    /// <param name="animalId">The identifier of the animal.</param>
    /// <exception cref="InvalidOperationException">Thrown if the animal is not found.</exception>
    public void RemoveAnimal(Guid animalId, Guid userId)
    {
        if (!this.IsManager(userId) && !this.IsAdminOrModerator())
        {
            throw new InvalidOperationException("Ви не маєте права видаляти тварин з цього притулку.");
        }

        var animal = this.animals.FirstOrDefault(a => a.Id == animalId);
        if (animal is null)
        {
            throw new KeyNotFoundException("Тварину з таким ID не знайдено у притулку.");
        }

        this.animals.Remove(animal);
        this.CurrentOccupancy--;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalRemovedFromShelterEvent(this.Id, animalId, this.CurrentOccupancy));
    }

    // AnimalAidRequest

    /// <summary>
    /// Adds a new animal aid request to the shelter.
    /// </summary>
    /// <param name="request">The request to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the shelter manager or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to add the request.</exception>
    public void AddAnimalAidRequest(AnimalAidRequest request, Guid requestingUserId)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Запит допомоги тварині не може бути null.");
        }

        if (!this.IsManager(requestingUserId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки менеджер притулку або адміністратор/модератор може додавати запит.");
        }

        this.animalAidRequests.Add(request);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalAidRequestAddedEvent(this.Id, request.Id));
    }

    /// <summary>
    /// Removes an animal aid request from the shelter.
    /// </summary>
    /// <param name="requestId">The ID of the request to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the shelter manager or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown when the request is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to remove the request.</exception>
    public void RemoveAnimalAidRequest(Guid requestId, Guid requestingUserId)
    {
        var request = this.animalAidRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null)
        {
            throw new InvalidOperationException("Запит допомоги тварині не знайдено.");
        }

        if (!this.IsManager(requestingUserId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки менеджер притулку або адміністратор/модератор може видаляти запит.");
        }

        this.animalAidRequests.Remove(request);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalAidRequestRemovedEvent(this.Id, requestId));
    }

    // Photos

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
        this.AddDomainEvent(new ShelterPhotoAddedEvent(this.Id, photoUrl));
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
            this.AddDomainEvent(new ShelterPhotoRemovedEvent(this.Id, photoUrl));
        }

        return removed;
    }

    // SocialMedia

    /// <summary>
    /// Adds or updates a social media link for the shelter.
    /// </summary>
    /// <param name="platform">The social media platform name (e.g. "Facebook").</param>
    /// <param name="url">The URL to the social media page.</param>
    /// <exception cref="ArgumentException">Thrown when platform or url is null or whitespace.</exception>
    public void AddOrUpdateSocialMedia(string platform, string url)
    {
        if (string.IsNullOrWhiteSpace(platform))
        {
            throw new ArgumentException("Назва платформи не може бути порожньою.", nameof(platform));
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL не може бути порожнім.", nameof(url));
        }

        this.socialMedia[platform] = url;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ShelterSocialMediaAddedOrUpdatedEvent(this.Id, platform, url));
    }

    /// <summary>
    /// Removes a social media link from the shelter.
    /// </summary>
    /// <param name="platform">The social media platform name to remove.</param>
    /// <returns>True if the link was removed; otherwise, false.</returns>
    public bool RemoveSocialMedia(string platform)
    {
        var removed = this.socialMedia.Remove(platform);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new ShelterSocialMediaRemovedEvent(this.Id, platform));
        }

        return removed;
    }

    // Donations

    /// <summary>
    /// Adds a new donation made by the shelter.
    /// </summary>
    /// <param name="donation">The donation to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="donation"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to add the donation.</exception>
    public void AddDonation(Donation donation, Guid requestingUserId)
    {
        if (donation is null)
        {
            throw new ArgumentNullException(nameof(donation), "Пожертва не може бути null.");
        }

        if (!this.IsManager(requestingUserId))
        {
            throw new UnauthorizedAccessException("Тільки менеджер може додавати пожертву.");
        }

        this.donations.Add(donation);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new DonationAddedToShelterEvent(this.Id, donation.Id));
    }

    /// <summary>
    /// Removes a donation made by the shelter.
    /// </summary>
    /// <param name="donationId">The ID of the donation to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown when the donation is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to remove the donation.</exception>
    public void RemoveDonation(Guid donationId, Guid requestingUserId)
    {
        var donation = this.donations.FirstOrDefault(d => d.Id == donationId);
        if (donation == null)
        {
            throw new InvalidOperationException("Пожертва не знайдена.");
        }

        if (!this.IsManager(requestingUserId))
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужої пожертви.");
        }

        this.donations.Remove(donation);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new DonationRemovedFromShelterEvent(this.Id, donationId));
    }

    // VolunteerTask

    /// <summary>
    /// Adds a new volunteer task to the shelter.
    /// </summary>
    /// <param name="task">The volunteer task to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the manager or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="task"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to add the task.</exception>
    public void AddVolunteerTask(VolunteerTask task, Guid requestingUserId)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task), "Завдання волонтера не може бути null.");
        }

        if (!this.IsManager(requestingUserId))
        {
            throw new UnauthorizedAccessException("Тільки менеджер може додавати завдання волонтера.");
        }

        this.volunteerTasks.Add(task);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new VolunteerTaskAddedToShelterEvent(this.Id, task.Id));
    }

    /// <summary>
    /// Removes a volunteer task from the shelter.
    /// </summary>
    /// <param name="taskId">The ID of the volunteer task to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the manager or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown when the task is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to remove the task.</exception>
    public void RemoveVolunteerTask(Guid taskId, Guid requestingUserId)
    {
        var task = this.volunteerTasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
        {
            throw new InvalidOperationException("Завдання волонтера не знайдено.");
        }

        if (!this.IsManager(requestingUserId))
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужого завдання волонтера.");
        }

        this.volunteerTasks.Remove(task);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new VolunteerTaskRemovedFromShelterEvent(this.Id, taskId));
    }

    // IoTDevice

    /// <summary>
    /// Adds a new IoT device to the shelter.
    /// </summary>
    /// <param name="device">The IoT device to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when device is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the device is already linked to the shelter.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not a manager, admin, or moderator.</exception>
    public void AddIoTDevice(IoTDevice device, Guid requestingUserId)
    {
        if (device is null)
        {
            throw new ArgumentNullException(nameof(device), "IoT-пристрій не може бути null.");
        }

        if (this.ioTDevices.Any(d => d.Id == device.Id))
        {
            throw new InvalidOperationException("Цей IoT-пристрій вже доданий до притулку.");
        }

        if (!this.IsManager(requestingUserId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для додавання IoT-пристрою.");
        }

        this.ioTDevices.Add(device);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new IoTDeviceAddedEvent(this.Id, device.Id));
    }

    /// <summary>
    /// Removes an IoT device from the shelter.
    /// </summary>
    /// <param name="deviceId">The ID of the device to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation.</param>
    /// <exception cref="InvalidOperationException">Thrown when the device is not found in the shelter.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not a manager, admin, or moderator.</exception>
    public void RemoveIoTDevice(Guid deviceId, Guid requestingUserId)
    {
        var device = this.ioTDevices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new InvalidOperationException("IoT-пристрій не знайдено у притулку.");
        }

        if (!this.IsManager(requestingUserId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення IoT-пристрою.");
        }

        this.ioTDevices.Remove(device);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new IoTDeviceRemovedEvent(this.Id, deviceId));
    }

    // Event

    /// <summary>
    /// Adds an event to the shelter.
    /// </summary>
    /// <param name="event">The event to add.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <exception cref="ArgumentNullException">Thrown when the event is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user does not have permission to add the event.</exception>
    public void AddEvent(Event @event, Guid userId)
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event), "Подія не може бути null.");
        }

        if (!this.IsManager(userId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("У вас немає прав для додавання події до притулку.");
        }

        this.events.Add(@event);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new ShelterEventAddedEvent(this.Id, @event.Id));
    }

    /// <summary>
    /// Removes an event from the shelter.
    /// </summary>
    /// <param name="eventId">The ID of the event to remove.</param>
    /// <param name="userId">The ID of the user performing the action.</param>
    /// <returns>True if the event was removed; otherwise, false.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user does not have permission to remove the event.</exception>
    public bool RemoveEvent(Guid eventId, Guid userId)
    {
        if (!this.IsManager(userId) && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("У вас немає прав для видалення події з притулку.");
        }

        var existingEvent = this.events.FirstOrDefault(e => e.Id == eventId);
        if (existingEvent is null)
        {
            return false;
        }

        this.events.Remove(existingEvent);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new ShelterEventRemovedEvent(this.Id, eventId));
        return true;
    }

    /// <summary>
    /// Checks if the shelter has free capacity.
    /// </summary>
    /// <returns>True if there is available capacity, otherwise false.</returns>
    public bool HasFreeCapacity() => this.CurrentOccupancy < this.Capacity;

    private bool IsManager(Guid userId) => this.ManagerId.HasValue && this.ManagerId.Value == userId;

    private bool IsAdminOrModerator() =>
        this.Manager != null && (this.Manager.Role == UserRole.Admin || this.Manager.Role == UserRole.Moderator);

}
