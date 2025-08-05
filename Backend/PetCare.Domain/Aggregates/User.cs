// <copyright file="User.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>
namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a user in the system.
/// </summary>
public sealed class User : BaseEntity
{
    private readonly List<AdoptionApplication> adoptionApplications = new();
    private readonly List<SuccessStory> successStories = new();
    private readonly List<AnimalAidRequest> animalAidRequests = new();
    private readonly List<Article> articles = new();
    private readonly List<ArticleComment> articleComments = new();
    private readonly List<Notification> notifications = new();
    private readonly List<GamificationReward> gamificationRewards = new();
    private readonly List<ShelterSubscription> shelterSubscriptions = new();

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

        return new User(
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
    }

    /// <summary>
    /// Updates the user's profile with the provided values.
    /// </summary>
    /// <param name="firstName">The new first name of the user, if provided. If null or whitespace, the first name remains unchanged.</param>
    /// <param name="lastName">The new last name of the user, if provided. If null or whitespace, the last name remains unchanged.</param>
    /// <param name="phone">The new phone number of the user, if provided. If null or whitespace, the phone number remains unchanged.</param>
    /// <param name="profilePhoto">The new URL of the user's profile photo, if provided. If null or whitespace, the profile photo remains unchanged.</param>
    /// <param name="language">The new preferred language of the user, if provided. If null or whitespace, the language remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="phone"/> is invalid according to the <see cref="PhoneNumber.Create"/> method.</exception>
    public void UpdateProfile(
        string? firstName = null,
        string? lastName = null,
        string? phone = null,
        string? profilePhoto = null,
        string? language = null)
    {
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
    }

    /// <summary>
    /// Adds the specified amount of points to the user's total points.
    /// </summary>
    /// <param name="amount">The number of points to add. If negative, no points are added.</param>
    public void AddPoints(int amount)
    {
        if (amount < 0)
        {
            return;
        }

        this.Points += amount;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deducts the specified amount of points from the user.
    /// </summary>
    /// <param name="amount">The number of points to deduct. Must be positive and less or equal to current points.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="amount"/> is negative or exceeds current points.</exception>
    public void DeductPoints(int amount)
    {
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
    }

    /// <summary>
    /// Sets the date and time of the user's last login.
    /// </summary>
    /// <param name="date">The date and time of the last login.</param>
    public void SetLastLogin(DateTime date)
    {
        this.LastLogin = date;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new shelter subscription to the user.
    /// </summary>
    /// <param name="subscription">The shelter subscription to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="subscription"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the subscription already exists.</exception>
    public void AddShelterSubscription(ShelterSubscription subscription)
    {
        if (subscription is null)
        {
            throw new ArgumentNullException(nameof(subscription), "Підписка не може бути null.");
        }

        if (this.shelterSubscriptions.Any(s => s.ShelterId == subscription.ShelterId))
        {
            throw new InvalidOperationException("Така підписка вже існує.");
        }

        this.shelterSubscriptions.Add(subscription);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the subscription date of an existing shelter subscription.
    /// </summary>
    /// <param name="shelterId">The shelter ID of the subscription to update.</param>
    /// <param name="newSubscribedAt">The new subscription date.</param>
    /// <returns>True if updated; otherwise false.</returns>
    public bool UpdateShelterSubscriptionDate(Guid shelterId, DateTime newSubscribedAt)
    {
        var subscription = this.shelterSubscriptions.FirstOrDefault(s => s.ShelterId == shelterId);
        if (subscription == null)
        {
            return false;
        }

        // Remove old subscription
        this.shelterSubscriptions.Remove(subscription);

        // Add new subscription with updated date
        var updatedSubscription = ShelterSubscription.Create(this.Id, shelterId);

        this.shelterSubscriptions.Add(updatedSubscription);
        this.UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Removes a shelter subscription from the user.
    /// </summary>
    /// <param name="shelterId">The shelter ID of the subscription to remove.</param>
    /// <returns>True if the subscription was removed; otherwise false.</returns>
    public bool RemoveShelterSubscription(Guid shelterId)
    {
        var subscription = this.shelterSubscriptions.FirstOrDefault(s => s.ShelterId == shelterId);
        if (subscription == null)
        {
            return false;
        }

        bool removed = this.shelterSubscriptions.Remove(subscription);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
        }

        return removed;
    }

    /// <summary>
    /// Adds gamification points reward to the user.
    /// </summary>
    /// <param name="reward">The gamification reward to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reward"/> is null.</exception>
    public void AddGamificationReward(GamificationReward reward)
    {
        if (reward is null)
        {
            throw new ArgumentNullException(nameof(reward), "Винагорода не може бути null.");
        }

        this.gamificationRewards.Add(reward);
        this.AddPoints(reward.Points);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new adoption application created by the user.
    /// </summary>
    /// <param name="application">The adoption application to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="application"/> is null.</exception>
    public void AddAdoptionApplication(AdoptionApplication application)
    {
        if (application is null)
        {
            throw new ArgumentNullException(nameof(application), "Заявка не може бути null.");
        }

        if (this.adoptionApplications.Any(a => a.Id == application.Id))
        {
            throw new InvalidOperationException("Заявка вже існує.");
        }

        this.adoptionApplications.Add(application);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an adoption application from the user's collection by its identifier.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the adoption application to remove.</param>
    /// <returns>
    /// True if the adoption application was found and removed; otherwise, false.
    /// </returns>
    public bool RemoveAdoptionApplication(Guid applicationId)
    {
        var application = this.adoptionApplications.FirstOrDefault(a => a.Id == applicationId);
        if (application == null)
        {
            return false;
        }

        bool removed = this.adoptionApplications.Remove(application);
        if (removed)
        {
            this.UpdatedAt = DateTime.UtcNow;
        }

        return removed;
    }

    /// <summary>
    /// Adds a new animal aid request created by the user.
    /// </summary>
    /// <param name="request">The animal aid request to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    public void AddAnimalAidRequest(AnimalAidRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Запит не може бути null.");
        }

        this.animalAidRequests.Add(request);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new article created by the user.
    /// </summary>
    /// <param name="article">The article to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="article"/> is null.</exception>
    public void AddArticle(Article article)
    {
        if (article is null)
        {
            throw new ArgumentNullException(nameof(article), "Стаття не може бути null.");
        }

        this.articles.Add(article);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new article comment created by the user.
    /// </summary>
    /// <param name="comment">The article comment to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comment"/> is null.</exception>
    public void AddArticleComment(ArticleComment comment)
    {
        if (comment is null)
        {
            throw new ArgumentNullException(nameof(comment), "Коментар не може бути null.");
        }

        this.articleComments.Add(comment);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new notification for the user.
    /// </summary>
    /// <param name="notification">The notification to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="notification"/> is null.</exception>
    public void AddNotification(Notification notification)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification), "Сповіщення не може бути null.");
        }

        this.notifications.Add(notification);
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a new success story created by the user.
    /// </summary>
    /// <param name="story">The success story to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="story"/> is null.</exception>
    public void AddSuccessStory(SuccessStory story)
    {
        if (story is null)
        {
            throw new ArgumentNullException(nameof(story), "Історія успіху не може бути null.");
        }

        this.successStories.Add(story);
        this.UpdatedAt = DateTime.UtcNow;
    }
}
