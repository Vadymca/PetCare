namespace PetCare.Domain.Events;

/// <summary>
/// Represents an event that occurs when a new user is created.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the event.</param>
public sealed record UserCreatedEvent(Guid UserId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that occurs when a user's email address has been confirmed.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose email address was confirmed.</param>
/// <param name="Email">The email address that has been confirmed for the user.</param>
public sealed record UserEmailConfirmedEvent(Guid UserId, string Email)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user's profile is updated.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose profile was updated.</param>
public sealed record UserProfileUpdatedEvent(Guid UserId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user's profile photo is changed.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose profile photo has changed.</param>
/// <param name="NewPhotoUrl">The URL of the new profile photo. Can be null if the photo was removed.</param>
public sealed record UserProfilePhotoChangedEvent(Guid UserId, string? NewPhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user's profile photo is removed.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose profile photo was removed.</param>
/// <param name="OldPhotoUrl">The URL of the profile photo that was removed. This value may be used to reference or clean up the previous image.</param>
public sealed record UserProfilePhotoRemovedEvent(Guid UserId, string OldPhotoUrl)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when points are added to a user's account.
/// </summary>
/// <param name="UserId">The unique identifier of the user to whom points were added.</param>
/// <param name="Amount">The number of points that were added to the user's account.</param>
public sealed record UserPointsAddedEvent(Guid UserId, int Amount)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when points are deducted from a user's account.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose points were deducted.</param>
/// <param name="Amount">The number of points that were deducted from the user's account.</param>
public sealed record UserPointsDeductedEvent(Guid UserId, int Amount)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user's password has been changed.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose password was changed.</param>
public sealed record UserPasswordChangedEvent(Guid UserId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user subscribes to a shelter.
/// </summary>
/// <param name="UserId">The unique identifier of the user who has subscribed to the shelter.</param>
/// <param name="ShelterId">The unique identifier of the shelter to which the user has subscribed.</param>
public sealed record ShelterSubscriptionAddedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user's subscription to a shelter is updated.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose subscription was updated.</param>
/// <param name="ShelterId">The unique identifier of the shelter associated with the updated subscription.</param>
public sealed record ShelterSubscriptionUpdatedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user is unsubscribed from a shelter.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose subscription was removed.</param>
/// <param name="ShelterId">The unique identifier of the shelter from which the user was unsubscribed.</param>
public sealed record ShelterSubscriptionRemovedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a gamification reward is added to a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who received the reward.</param>
/// <param name="RewardId">The unique identifier of the reward that was added.</param>
/// <param name="Points">The number of points associated with the added reward.</param>
public sealed record GamificationRewardAddedEvent(Guid UserId, Guid RewardId, int Points)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a gamification reward is removed from a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user from whom the reward was removed.</param>
/// <param name="RewardId">The unique identifier of the reward that was removed.</param>
public sealed record GamificationRewardRemovedEvent(Guid UserId, Guid RewardId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new adoption application is added by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who submitted the adoption application.</param>
/// <param name="ApplicationId">The unique identifier of the adoption application that was added.</param>
public sealed record AdoptionApplicationAddedEvent(Guid UserId, Guid ApplicationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when an adoption application is removed by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who removed the adoption application.</param>
/// <param name="ApplicationId">The unique identifier of the adoption application that was removed.</param>
public sealed record AdoptionApplicationRemovedEvent(Guid UserId, Guid ApplicationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new animal aid request is added by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who created the animal aid request.</param>
/// <param name="RequestId">The unique identifier of the newly added animal aid request.</param>
public sealed record AnimalAidRequestAddedEvent(Guid UserId, Guid RequestId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when an animal aid request is removed by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who removed the animal aid request.</param>
/// <param name="RequestId">The unique identifier of the animal aid request that was removed.</param>
public sealed record AnimalAidRequestRemovedEvent(Guid UserId, Guid RequestId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a new article is added by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who added the article.</param>
/// <param name="ArticleId">The unique identifier of the article that was added.</param>
public sealed record ArticleAddedEvent(Guid UserId, Guid ArticleId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when an article is removed by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who removed the article.</param>
/// <param name="ArticleId">The unique identifier of the article that was removed.</param>
public sealed record ArticleRemovedEvent(Guid UserId, Guid ArticleId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that occurs when a comment is added to an article.
/// </summary>
/// <param name="UserId">The unique identifier of the user who added the comment.</param>
/// <param name="CommentId">The unique identifier of the comment that was added.</param>
public sealed record ArticleCommentAddedEvent(Guid UserId, Guid CommentId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a comment is removed from an article.
/// </summary>
/// <param name="UserId">The unique identifier of the user who removed the comment.</param>
/// <param name="CommentId">The unique identifier of the comment that was removed.</param>
public sealed record ArticleCommentRemovedEvent(Guid UserId, Guid CommentId)
    : DomainEvent;

/// <summary>
/// Represents the event that occurs when a notification is added for a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user to whom the notification was added.</param>
/// <param name="NotificationId">The unique identifier of the notification that was added.</param>
public sealed record NotificationAddedEvent(Guid UserId, Guid NotificationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a notification is removed for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the removed notification.</param>
/// <param name="NotificationId">The unique identifier of the notification that was removed.</param>
public sealed record NotificationRemovedEvent(Guid UserId, Guid NotificationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a user adds a new success story.
/// </summary>
/// <param name="UserId">The unique identifier of the user who added the success story.</param>
/// <param name="StoryId">The unique identifier of the success story that was added.</param>
public sealed record SuccessStoryAddedEvent(Guid UserId, Guid StoryId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a success story is removed from a user's profile.
/// </summary>
/// <param name="UserId">The unique identifier of the user from whose profile the success story was removed.</param>
/// <param name="StoryId">The unique identifier of the success story that was removed.</param>
public sealed record SuccessStoryRemovedEvent(Guid UserId, Guid StoryId)
    : DomainEvent;

/// <summary>
/// Represents the event that occurs when a lost pet is added by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who reported the lost pet.</param>
/// <param name="LostPetId">The unique identifier of the lost pet that was added.</param>
public sealed record LostPetAddedEvent(Guid UserId, Guid LostPetId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a lost pet is removed by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who removed the lost pet.</param>
/// <param name="LostPetId">The unique identifier of the lost pet that was removed.</param>
public sealed record LostPetRemovedEvent(Guid UserId, Guid LostPetId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that occurs when a new event is added by a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user who added the event.</param>
/// <param name="EventId">The unique identifier of the event that was added.</param>
public sealed record EventAddedEvent(Guid UserId, Guid EventId)
    : DomainEvent;

/// <summary>
/// Represents a domain event that indicates an event has been removed for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the removed event.</param>
/// <param name="EventId">The unique identifier of the event that was removed.</param>
public sealed record EventRemovedEvent(Guid UserId, Guid EventId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a donation is added for a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the donation.</param>
/// <param name="DonationId">The unique identifier of the donation that was added.</param>
public sealed record DonationAddedEvent(Guid UserId, Guid DonationId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a donation is removed for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the removed donation.</param>
/// <param name="DonationId">The unique identifier of the donation that was removed.</param>
public sealed record DonationRemovedEvent(Guid UserId, Guid DonationId)
    : DomainEvent;

/// <summary>
/// Represents a domain event indicating that a user's last login time has been set.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose last login time was updated.</param>
/// <param name="LastLogin">The date and time, in UTC, when the user last logged in.</param>
public sealed record UserLastLoginSetEvent(Guid UserId, DateTime LastLogin)
    : DomainEvent;
