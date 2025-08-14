namespace PetCare.Domain.Aggregates;

using PetCare.Domain.Abstractions;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a user in the system.
/// </summary>
public sealed class User : AggregateRoot
{
    private readonly List<AdoptionApplication> adoptionApplications = new();
    private readonly List<SuccessStory> successStories = new();
    private readonly List<AnimalAidRequest> animalAidRequests = new();
    private readonly List<Article> articles = new();
    private readonly List<ArticleComment> articleComments = new();
    private readonly List<Notification> notifications = new();
    private readonly List<GamificationReward> gamificationRewards = new();
    private readonly List<ShelterSubscription> shelterSubscriptions = new();
    private readonly List<LostPet> lostPets = new();
    private readonly List<Event> events = new();
    private readonly List<Donation> donations = new();
    private readonly List<VolunteerTaskAssignment> volunteerTaskAssignments = new();
    private readonly List<EventParticipant> eventParticipations = new();
    private readonly List<AnimalSubscription> animalSubscriptions = new();

    private User()
    {
        this.Email = Email.Create("default@example.com");
        this.PasswordHash = string.Empty;
        this.FirstName = string.Empty;
        this.LastName = string.Empty;
        this.Phone = PhoneNumber.Create("+000000000000");
        this.Preferences = new Dictionary<string, string>();
        this.Language = "uk";
    }

    private User(
        Email email,
        string passwordHash,
        string firstName,
        string lastName,
        PhoneNumber phone,
        UserRole role,
        Dictionary<string, string> preferences,
        int points,
        DateTime? lastLogin,
        string? profilePhoto,
        string language)
    {
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Phone = phone;
        this.Role = role;
        this.Preferences = preferences ?? new();
        this.Points = points;
        this.LastLogin = lastLogin;
        this.ProfilePhoto = profilePhoto;
        this.Language = language;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets the hashed password of the user.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public PhoneNumber Phone { get; private set; }

    /// <summary>
    /// Gets the role of the user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the preferences of the user.
    /// </summary>
    public Dictionary<string, string> Preferences { get; private set; }

    /// <summary>
    /// Gets the points accumulated by the user.
    /// </summary>
    public int Points { get; private set; }

    /// <summary>
    /// Gets the date and time of the user's last login, if any. Can be null.
    /// </summary>
    public DateTime? LastLogin { get; private set; }

    /// <summary>
    /// Gets the URL of the user's profile photo, if any. Can be null.
    /// </summary>
    public string? ProfilePhoto { get; private set; }

    /// <summary>
    /// Gets the preferred language of the user.
    /// </summary>
    public string Language { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the shelter subscriptions of the user.
    /// </summary>
    public IReadOnlyCollection<ShelterSubscription> ShelterSubscriptions => this.shelterSubscriptions.AsReadOnly();

    /// <summary>
    /// Gets the gamification rewards of the user.
    /// </summary>
    public IReadOnlyCollection<GamificationReward> GamificationRewards => this.gamificationRewards.AsReadOnly();

    /// <summary>
    /// Gets the adoption applications created by the user.
    /// </summary>
    public IReadOnlyCollection<AdoptionApplication> AdoptionApplications => this.adoptionApplications.AsReadOnly();

    /// <summary>
    /// Gets the animal aid requests created by the user.
    /// </summary>
    public IReadOnlyCollection<AnimalAidRequest> AnimalAidRequests => this.animalAidRequests.AsReadOnly();

    /// <summary>
    /// Gets the articles created by the user.
    /// </summary>
    public IReadOnlyCollection<Article> Articles => this.articles.AsReadOnly();

    /// <summary>
    /// Gets the comments created by the user.
    /// </summary>
    public IReadOnlyCollection<ArticleComment> ArticleComments => this.articleComments.AsReadOnly();

    /// <summary>
    /// Gets the notifications sent to the user.
    /// </summary>
    public IReadOnlyCollection<Notification> Notifications => this.notifications.AsReadOnly();

    /// <summary>
    /// Gets the success stories created by the user.
    /// </summary>
    public IReadOnlyCollection<SuccessStory> SuccessStories => this.successStories.AsReadOnly();

    /// <summary>
    /// Gets the collection of lost pets reported by the user.
    /// </summary>
    public IReadOnlyCollection<LostPet> LostPets => this.lostPets.AsReadOnly();

    /// <summary>
    /// Gets the collection of events created by the user.
    /// </summary>
    public IReadOnlyCollection<Event> Events => this.events.AsReadOnly();

    /// <summary>
    /// Gets the collection of donations made by the user.
    /// </summary>
    public IReadOnlyCollection<Donation> Donations => this.donations.AsReadOnly();

    /// <summary>
    /// Gets the participants of the event.
    /// </summary>
    public IReadOnlyCollection<EventParticipant> EventParticipations => this.eventParticipations.AsReadOnly();

    /// <summary>
    /// Gets the volunteer task assignments of the user.
    /// </summary>
    public IReadOnlyCollection<VolunteerTaskAssignment> VolunteerTaskAssignments =>
        this.volunteerTaskAssignments.AsReadOnly();

    /// <summary>
    /// Gets the animalSubscriptions of the user.
    /// </summary>
    public IReadOnlyCollection<AnimalSubscription> AnimalSubscriptions => this.animalSubscriptions.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="User"/> instance with the specified parameters.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="passwordHash">The hashed password of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="phone">The phone number of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <param name="preferences">The preferences of the user, if any. Can be null.</param>
    /// <param name="points">The points accumulated by the user. Defaults to 0.</param>
    /// <param name="lastLogin">The date and time of the user's last login, if any. Can be null.</param>
    /// <param name="profilePhoto">The URL of the user's profile photo, if any. Can be null.</param>
    /// <param name="language">The preferred language of the user. Defaults to "uk".</param>
    /// <returns>A new instance of <see cref="User"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="passwordHash"/>, <paramref name="firstName"/>, <paramref name="lastName"/>, or <paramref name="language"/> is null, whitespace, or exceeds length limits, or when <paramref name="points"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> or <paramref name="phone"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        string phone,
        UserRole role,
        Dictionary<string, string>? preferences = null,
        int points = 0,
        DateTime? lastLogin = null,
        string? profilePhoto = null,
        string language = "uk")
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Хеш-пароль не може бути порожнім", nameof(passwordHash));
        }

        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 50)
        {
            throw new ArgumentException("Ім'я невірне", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 50)
        {
            throw new ArgumentException("Прізвище невірне", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(language) || language.Length > 10)
        {
            throw new ArgumentException("Мова невірна", nameof(language));
        }

        if (points < 0)
        {
            throw new ArgumentException("Бали не можуть бути від'ємними", nameof(points));
        }

        var user = new User(
            Email.Create(email),
            passwordHash,
            firstName,
            lastName,
            PhoneNumber.Create(phone),
            role,
            preferences ?? new Dictionary<string, string>(),
            points,
            lastLogin,
            profilePhoto,
            language);

        user.AddDomainEvent(new UserCreatedEvent(user.Id));
        return user;
    }

    /// <summary>
    /// Updates the user's profile with the provided values.
    /// </summary>
    /// <param name="firstName">The new first name of the user, if provided. If null or whitespace, the first name remains unchanged.</param>
    /// <param name="lastName">The new last name of the user, if provided. If null or whitespace, the last name remains unchanged.</param>
    /// <param name="phone">The new phone number of the user, if provided. If null or whitespace, the phone number remains unchanged.</param>
    /// <param name="profilePhoto">The new URL of the user's profile photo, if provided. If null or whitespace, the profile photo remains unchanged.</param>
    /// <param name="language">The new preferred language of the user, if provided. If null or whitespace, the language remains unchanged.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="phone"/> is invalid according to the <see cref="PhoneNumber.Create"/> method.</exception>
    public void UpdateProfile(
        string? firstName = null,
        string? lastName = null,
        string? phone = null,
        string? profilePhoto = null,
        string? language = null,
        Guid requestingUserId = default)
    {
        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для оновлення профілю іншого користувача.");
        }

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            this.FirstName = firstName;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            this.LastName = lastName;
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            this.Phone = PhoneNumber.Create(phone);
        }

        if (!string.IsNullOrWhiteSpace(profilePhoto))
        {
            this.ProfilePhoto = profilePhoto;
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            this.Language = language;
        }

        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserProfileUpdatedEvent(this.Id));
    }

    /// <summary>
    /// Updates the profile photo for the current user.
    /// </summary>
    /// <param name="fileStorage">The file storage service used to upload and delete files.</param>
    /// <param name="fileStream">The stream containing the new profile photo data.</param>
    /// <param name="fileName">The name of the new profile photo file.</param>
    /// <param name="config">The configuration for profile photo validation (size and allowed extensions).</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the requesting user ID does not match the current user's ID.</exception>
    public async Task UpdateProfilePhotoAsync(
        IFileStorageService fileStorage,
        Stream fileStream,
        string fileName,
        ProfilePhotoConfig config,
        Guid requestingUserId)
    {
        if (this.Id != requestingUserId && !this.IsAdminOrModerator())
        {
            throw new ArgumentException("Можна змінювати лише власне фото");
        }

        if (!string.IsNullOrWhiteSpace(this.ProfilePhoto))
        {
            await fileStorage.DeleteAsync(this.ProfilePhoto);
        }

        var newPhotoUrl = await fileStorage.UploadAsync(fileStream, fileName, config.maxSizeBytes, config.allowedExtensions);
        this.ProfilePhoto = newPhotoUrl;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserProfilePhotoChangedEvent(this.Id, newPhotoUrl));
    }

    /// <summary>
    /// Removes the current profile photo.
    /// </summary>
    /// <param name="fileStorage">The file storage service used to delete the profile photo.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveProfilePhotoAsync(
        IFileStorageService fileStorage,
        Guid requestingUserId)
    {
        if (this.Id != requestingUserId && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Можна видаляти лише власне фото або мати права адміністратора/модератора.");
        }

        if (string.IsNullOrWhiteSpace(this.ProfilePhoto))
        {
            return;
        }

        await fileStorage.DeleteAsync(this.ProfilePhoto);
        this.ProfilePhoto = null;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserProfilePhotoRemovedEvent(this.Id));
    }

    /// <summary>
    /// Adds the specified amount of points to the user's total points.
    /// </summary>
    /// <param name="amount">The number of points to add. If negative, no points are added.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID.</param>
    public void AddPoints(int amount, Guid requestingUserId)
    {
        if (!this.IsAdmin())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор може додавати бали.");
        }

        if (amount < 0)
        {
            return;
        }

        this.Points += amount;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserPointsAddedEvent(this.Id, amount));
    }

    /// <summary>
    /// Deducts the specified amount of points from the user.
    /// </summary>
    /// <param name="amount">The number of points to deduct. Must be positive and less or equal to current points.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="amount"/> is negative or exceeds current points.</exception>
    public void DeductPoints(int amount, Guid requestingUserId)
    {
        if (!this.IsAdmin())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор може віднімати бали.");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Сума віднімання балів не може бути від'ємною.", nameof(amount));
        }

        if (amount > this.Points)
        {
            throw new ArgumentException("Недостатньо балів для списання.", nameof(amount));
        }

        this.Points -= amount;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserPointsDeductedEvent(this.Id, amount));
    }

    /// <summary>
    /// Sets the date and time of the user's last login.
    /// </summary>
    /// <param name="date">The date and time of the last login.</param>
    public void SetLastLogin(DateTime date)
    {
        this.LastLogin = date;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserLastLoginSetEvent(this.Id, date));
    }

    /// <summary>
    /// Changes the user's password hash.
    /// Only the user themselves or an admin/moderator can perform this action.
    /// </summary>
    /// <param name="newPasswordHash">The new password hash. Cannot be null or whitespace.</param>
    /// <param name="requestingUserId">The ID of the user requesting the password change.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newPasswordHash"/> is null or whitespace.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not the owner or an admin/moderator.</exception>
    public void ChangePassword(string newPasswordHash, Guid requestingUserId)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new ArgumentException("Хеш пароля не може бути порожнім.", nameof(newPasswordHash));
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для зміни пароля іншого користувача.");
        }

        this.PasswordHash = newPasswordHash;
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new UserPasswordChangedEvent(this.Id));
    }

    // ShelterSubscription CRUD з ролями

    /// <summary>
    /// Adds a new shelter subscription to the user.
    /// </summary>
    /// <param name="subscription">The shelter subscription to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="subscription"/> is null.</exception>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="InvalidOperationException">Thrown when the subscription already exists.</exception>
    public void AddShelterSubscription(ShelterSubscription subscription, Guid requestingUserId)
    {
        if (subscription is null)
        {
            throw new ArgumentNullException(nameof(subscription), "Підписка не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може додавати підписку.");
        }

        if (this.shelterSubscriptions.Any(s => s.ShelterId == subscription.ShelterId))
        {
            throw new InvalidOperationException("Така підписка вже існує.");
        }

        this.shelterSubscriptions.Add(subscription);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ShelterSubscriptionAddedEvent(this.Id, subscription.ShelterId));
    }

    /// <summary>
    /// Updates the subscription date of an existing shelter subscription.
    /// </summary>
    /// <param name="shelterId">The shelter ID of the subscription to update.</param>
    /// <param name="newSubscribedAt">The new subscription date.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <returns>True if updated; otherwise false.</returns>
    public bool UpdateShelterSubscriptionDate(Guid shelterId, DateTime newSubscribedAt, Guid requestingUserId)
    {
        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може оновлювати підписку.");
        }

        var subscription = this.shelterSubscriptions.FirstOrDefault(s => s.ShelterId == shelterId);
        if (subscription == null)
        {
            return false;
        }

        this.shelterSubscriptions.Remove(subscription);
        var updatedSubscription = ShelterSubscription.Create(this.Id, shelterId);
        this.shelterSubscriptions.Add(updatedSubscription);
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new ShelterSubscriptionUpdatedEvent(this.Id, shelterId));

        return true;
    }

    /// <summary>
    /// Removes a shelter subscription from the user.
    /// </summary>
    /// <param name="shelterId">The shelter ID of the subscription to remove.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <returns>True if the subscription was removed; otherwise false.</returns>
    public bool RemoveShelterSubscription(Guid shelterId, Guid requestingUserId)
    {
        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може видаляти підписку.");
        }

        var subscription = this.shelterSubscriptions.FirstOrDefault(s => s.ShelterId == shelterId);
        if (subscription == null)
        {
            return false;
        }

        bool removed = this.shelterSubscriptions.Remove(subscription);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new ShelterSubscriptionRemovedEvent(this.Id, shelterId));
        }

        return removed;
    }

    // GamificationRewards

    /// <summary>
    /// Adds gamification points reward to the user.
    /// </summary>
    /// <param name="reward">The gamification reward to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reward"/> is null.</exception>
    public void AddGamificationReward(GamificationReward reward, Guid requestingUserId)
    {
        if (reward is null)
        {
            throw new ArgumentNullException(nameof(reward), "Винагорода не може бути null.");
        }

        if (!this.IsAdmin())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор може додавати винагороди.");
        }

        this.gamificationRewards.Add(reward);
        this.AddPoints(reward.Points, requestingUserId);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new GamificationRewardAddedEvent(this.Id, reward.Id, reward.Points));
    }

    /// <summary>
    /// Removes a gamification reward from the user.
    /// Only the user themselves or admins/moderators can perform this action.
    /// </summary>
    /// <param name="rewardId">The unique identifier of the gamification reward to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <returns>True if the reward was found and removed; otherwise, false.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have permission.</exception>
    public bool RemoveGamificationReward(Guid rewardId, Guid requestingUserId)
    {
        if (!this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор/модератор може видаляти винагороду.");
        }

        var reward = this.gamificationRewards.FirstOrDefault(r => r.Id == rewardId);
        if (reward == null)
        {
            return false;
        }

        bool removed = this.gamificationRewards.Remove(reward);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;

            this.AddDomainEvent(new GamificationRewardRemovedEvent(this.Id, rewardId));
        }

        return removed;
    }

    // AdoptionApplications

    /// <summary>
    /// Adds a new adoption application created by the user.
    /// </summary>
    /// <param name="application">The adoption application to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="application"/> is null.</exception>
    public void AddAdoptionApplication(AdoptionApplication application, Guid requestingUserId)
    {
        if (application is null)
        {
            throw new ArgumentNullException(nameof(application), "Заявка не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник заявки або адміністратор/модератор може додавати заявку.");
        }

        if (this.adoptionApplications.Any(a => a.Id == application.Id))
        {
            throw new InvalidOperationException("Заявка вже існує.");
        }

        this.adoptionApplications.Add(application);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AdoptionApplicationAddedEvent(this.Id, application.Id));
    }

    /// <summary>
    /// Removes an adoption application from the user's collection by its identifier.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the adoption application to remove.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <returns>
    /// True if the adoption application was found and removed; otherwise, false.
    /// </returns>
    public bool RemoveAdoptionApplication(Guid applicationId, Guid requestingUserId)
    {
        var application = this.adoptionApplications.FirstOrDefault(a => a.Id == applicationId);
        if (application == null)
        {
            return false;
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник заявки або адміністратор/модератор може видаляти заявку.");
        }

        bool removed = this.adoptionApplications.Remove(application);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new AdoptionApplicationRemovedEvent(this.Id, applicationId));
        }

        return removed;
    }

    // AnimalAidRequests

    /// <summary>
    /// Adds a new animal aid request created by the user.
    /// </summary>
    /// <param name="request">The animal aid request to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    public void AddAnimalAidRequest(AnimalAidRequest request, Guid requestingUserId)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Запит не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник запиту або адміністратор/модератор може додавати запит.");
        }

        this.animalAidRequests.Add(request);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalAidRequestAddedEvent(this.Id, request.Id));
    }

    /// <summary>
    /// Removes an animal aid request.
    /// Only admins or moderators can perform this action.
    /// </summary>
    /// <param name="requestId">The unique identifier of the animal aid request to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the request is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have admin or moderator rights.</exception>
    public void RemoveAnimalAidRequestAsAdmin(Guid requestId, Guid requestingUserId)
    {
        var request = this.animalAidRequests.FirstOrDefault(r => r.Id == requestId);
        if (request == null)
        {
            throw new InvalidOperationException("Запит не знайдено.");
        }

        if (!this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення запиту.");
        }

        this.animalAidRequests.Remove(request);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new AnimalAidRequestRemovedEvent(this.Id, requestId));
    }

    // Articles

    /// <summary>
    /// Adds a new article created by the user.
    /// </summary>
    /// <param name="article">The article to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="article"/> is null.</exception>
    public void AddArticle(Article article, Guid requestingUserId)
    {
        if (article is null)
        {
            throw new ArgumentNullException(nameof(article), "Стаття не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки автор статті або адміністратор/модератор може додавати статтю.");
        }

        this.articles.Add(article);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ArticleAddedEvent(this.Id, article.Id));
    }

    /// <summary>
    /// Removes an article created by the user.
    /// Only the article owner or admins/moderators can perform this action.
    /// </summary>
    /// <param name="articleId">The unique identifier of the article to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the article is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have permission.</exception>
    public void RemoveArticle(Guid articleId, Guid requestingUserId)
    {
        var article = this.articles.FirstOrDefault(a => a.Id == articleId);
        if (article == null)
        {
            throw new InvalidOperationException("Стаття не знайдена.");
        }

        bool isOwner = requestingUserId == this.Id;
        if (!isOwner && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужої статті.");
        }

        this.articles.Remove(article);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ArticleRemovedEvent(this.Id, articleId));
    }

    // ArticleComments

    /// <summary>
    /// Adds a new article comment created by the user.
    /// </summary>
    /// <param name="comment">The article comment to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
    public void AddArticleComment(ArticleComment comment, Guid requestingUserId)
    {
        if (comment is null)
        {
            throw new ArgumentNullException(nameof(comment), "Коментар не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки автор коментаря або адміністратор/модератор може додавати коментар.");
        }

        this.articleComments.Add(comment);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new ArticleCommentAddedEvent(this.Id, comment.Id));
    }

    /// <summary>
    /// Removes an article comment created by the user.
    /// Only the user themselves or admins/moderators can perform this action.
    /// </summary>
    /// <param name="commentId">The unique identifier of the article comment to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <returns>True if the comment was found and removed; otherwise, false.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have permission.</exception>
    public bool RemoveArticleComment(Guid commentId, Guid requestingUserId)
    {
        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може видаляти коментар.");
        }

        var comment = this.articleComments.FirstOrDefault(c => c.Id == commentId);
        if (comment == null)
        {
            return false;
        }

        bool removed = this.articleComments.Remove(comment);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new ArticleCommentRemovedEvent(this.Id, commentId));
        }

        return removed;
    }

    // Notifications

    /// <summary>
    /// Adds a new notification for the user.
    /// </summary>
    /// <param name="notification">The notification to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="notification"/> is null.</exception>
    public void AddNotification(Notification notification, Guid requestingUserId)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification), "Сповіщення не може бути null.");
        }

        if (!this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор або модератор може додавати сповіщення.");
        }

        this.notifications.Add(notification);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new NotificationAddedEvent(this.Id, notification.Id));
    }

    /// <summary>
    /// Removes a notification from the user.
    /// Only the user themselves or admins/moderators can perform this action.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <returns>True if the notification was found and removed; otherwise, false.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have permission.</exception>
    public bool RemoveNotification(Guid notificationId, Guid requestingUserId)
    {
        if (!this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки адміністратор або модератор може видаляти сповіщення.");
        }

        var notification = this.notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification == null)
        {
            return false;
        }

        bool removed = this.notifications.Remove(notification);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.AddDomainEvent(new NotificationRemovedEvent(this.Id, notificationId));
        }

        return removed;
    }

    // SuccessStories

    /// <summary>
    /// Adds a new success story created by the user.
    /// </summary>
    /// <param name="story">The success story to add.</param>
    /// <param name = "requestingUserId" > The ID of the user requesting the update.Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="story"/> is null.</exception>
    public void AddSuccessStory(SuccessStory story, Guid requestingUserId)
    {
        if (story is null)
        {
            throw new ArgumentNullException(nameof(story), "Історія успіху не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки автор історії або адміністратор/модератор може додавати історію успіху.");
        }

        if (requestingUserId == this.Id)
        {
            story.Unpublish();
        }
        else
        {
            story.Publish();
        }

        this.successStories.Add(story);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new SuccessStoryAddedEvent(this.Id, story.Id));
    }

    /// <summary>
    /// Removes a success story by its ID. Only an admin or moderator can perform this action.
    /// </summary>
    /// <param name="storyId">The unique identifier of the success story to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the success story is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not an admin or moderator.</exception>
    public void RemoveSuccessStoryAsAdmin(Guid storyId, Guid requestingUserId)
    {
        var story = this.successStories.FirstOrDefault(s => s.Id == storyId);
        if (story == null)
        {
            throw new InvalidOperationException("Історія успіху не знайдена.");
        }

        if (!this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення історії успіху.");
        }

        this.successStories.Remove(story);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new SuccessStoryRemovedEvent(this.Id, storyId));
    }

    // LostPets

    /// <summary>
    /// Adds a lost pet report to the user's collection.
    /// </summary>
    /// <param name="lostPet">The lost pet report to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="lostPet"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to add the lost pet.</exception>
    public void AddLostPet(LostPet lostPet, Guid requestingUserId)
    {
        if (lostPet is null)
        {
            throw new ArgumentNullException(nameof(lostPet), "Звіт про загублену тварину не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може додавати звіт про загублену тварину.");
        }

        this.lostPets.Add(lostPet);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new LostPetAddedEvent(this.Id, lostPet.Id));
    }

    /// <summary>
    /// Removes a lost pet report from the user's collection.
    /// </summary>
    /// <param name="lostPetId">The ID of the lost pet report to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown when the lost pet report is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to remove the lost pet report.</exception>
    public void RemoveLostPet(Guid lostPetId, Guid requestingUserId)
    {
        var lostPet = this.lostPets.FirstOrDefault(lp => lp.Id == lostPetId);
        if (lostPet == null)
        {
            throw new InvalidOperationException("Звіт про загублену тварину не знайдено.");
        }

        bool isOwner = requestingUserId == this.Id;
        if (!isOwner && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужого звіту про загублену тварину.");
        }

        this.lostPets.Remove(lostPet);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new LostPetRemovedEvent(this.Id, lostPetId));
    }

    // Events

    /// <summary>
    /// Adds a new event created by the user.
    /// </summary>
    /// <param name="eventItem">The event to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventItem"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to add the event.</exception>
    public void AddEvent(Event eventItem, Guid requestingUserId)
    {
        if (eventItem is null)
        {
            throw new ArgumentNullException(nameof(eventItem), "Подія не може бути null.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може додавати подію.");
        }

        this.events.Add(eventItem);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new EventAddedEvent(this.Id, eventItem.Id));
    }

    /// <summary>
    /// Removes an event created by the user.
    /// </summary>
    /// <param name="eventId">The ID of the event to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown when the event is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user is not authorized to remove the event.</exception>
    public void RemoveEvent(Guid eventId, Guid requestingUserId)
    {
        var eventItem = this.events.FirstOrDefault(e => e.Id == eventId);
        if (eventItem == null)
        {
            throw new InvalidOperationException("Подія не знайдена.");
        }

        bool isOwner = requestingUserId == this.Id;
        if (!isOwner && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужої події.");
        }

        this.events.Remove(eventItem);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new EventRemovedEvent(this.Id, eventId));
    }

    // Donations

    /// <summary>
    /// Adds a new donation made by the user.
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

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може додавати пожертву.");
        }

        this.donations.Add(donation);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new DonationAddedEvent(this.Id, donation.Id));
    }

    /// <summary>
    /// Removes a donation made by the user.
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

        bool isOwner = requestingUserId == this.Id;
        if (!isOwner && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення чужої пожертви.");
        }

        this.donations.Remove(donation);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new DonationRemovedEvent(this.Id, donationId));
    }

    // VolunteerTaskAssignment

    /// <summary>
    /// Adds a new volunteer task assignment for the user.
    /// </summary>
    /// <param name="assignment">The assignment entity to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="ArgumentNullException">Thrown if assignment is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the assignment already exists.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the requesting user is not authorized.</exception>
    public void AddVolunteerTaskAssignment(VolunteerTaskAssignment assignment, Guid requestingUserId)
    {
        if (assignment is null)
        {
            throw new ArgumentNullException(nameof(assignment), "Призначення волонтера не може бути null.");
        }

        if (this.volunteerTaskAssignments.Any(a => a.Id == assignment.Id))
        {
            throw new InvalidOperationException("Це призначення вже додано для користувача.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Тільки власник або адміністратор/модератор може додавати призначення.");
        }

        this.volunteerTaskAssignments.Add(assignment);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new VolunteerTaskAssignmentAddedEvent(this.Id, assignment.Id));
    }

    /// <summary>
    /// Removes a volunteer task assignment from the user.
    /// </summary>
    /// <param name="assignmentId">The ID of the assignment to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must be the owner or admin/moderator.</param>
    /// <exception cref="InvalidOperationException">Thrown if the assignment is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the requesting user is not authorized.</exception>
    public void RemoveVolunteerTaskAssignment(Guid assignmentId, Guid requestingUserId)
    {
        var assignment = this.volunteerTaskAssignments.FirstOrDefault(a => a.Id == assignmentId);
        if (assignment == null)
        {
            throw new InvalidOperationException("Призначення волонтера не знайдено.");
        }

        if (requestingUserId != this.Id && !this.IsAdminOrModerator())
        {
            throw new UnauthorizedAccessException("Недостатньо прав для видалення призначення.");
        }

        this.volunteerTaskAssignments.Remove(assignment);
        this.UpdatedAt = DateTime.UtcNow;
        this.AddDomainEvent(new VolunteerTaskAssignmentRemovedEvent(this.Id, assignmentId));
    }

    /// <summary>
    /// Determines whether the user can create an article.
    /// </summary>
    /// <returns>Always returns true, Users of any role can create articles.</returns>
    public bool CanCreateArticle() => true;

    /// <summary>
    /// Determines whether the user can delete an article.
    /// </summary>
    /// <returns>True, if the user has the Administrator role; otherwise — false.</returns>
    public bool CanDeleteArticle() => this.IsAdmin();

    /// <summary>
    /// Determines whether the user can moderate comments.
    /// </summary>
    /// <returns>True if the user is an Administrator or Moderator; otherwise, false.</returns>
    public bool CanModerateComments() => this.IsAdminOrModerator();

    /// <summary>
    /// Determines whether the user can manage other users.
    /// </summary>
    /// <returns>True, if the user has the Administrator role; otherwise — false.</returns>
    public bool CanManageUsers() => this.IsAdmin();

    /// <summary>
    /// Determines whether the user can submit an adoption application.
    /// </summary>
    /// <returns>Always returns true, users of any role can apply for adoption.</returns>
    public bool CanSubmitAdoptionApplication() => true;

    private bool IsAdmin() => this.Role == UserRole.Admin;

    private bool IsModerator() => this.Role == UserRole.Moderator;

    private bool IsAdminOrModerator() => this.IsAdmin() || this.IsModerator();
}
