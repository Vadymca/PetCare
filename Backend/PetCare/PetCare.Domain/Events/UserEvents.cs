namespace PetCare.Domain.Events;

public sealed record UserCreatedEvent(Guid userId)
    : DomainEvent;

public sealed record UserProfileUpdatedEvent(Guid userId)
    : DomainEvent;

public sealed record UserProfilePhotoChangedEvent(Guid userId, string? newPhotoUrl)
    : DomainEvent;

public sealed record UserProfilePhotoRemovedEvent(Guid userId)
    : DomainEvent;

public sealed record UserPointsAddedEvent(Guid userId, int amount)
    : DomainEvent;

public sealed record UserPointsDeductedEvent(Guid userId, int amount)
    : DomainEvent;

public sealed record UserPasswordChangedEvent(Guid userId)
    : DomainEvent;

public sealed record ShelterSubscriptionAddedEvent(Guid userId, Guid shelterId)
    : DomainEvent;

public sealed record ShelterSubscriptionUpdatedEvent(Guid userId, Guid shelterId)
    : DomainEvent;

public sealed record ShelterSubscriptionRemovedEvent(Guid userId, Guid shelterId)
    : DomainEvent;

public sealed record GamificationRewardAddedEvent(Guid userId, Guid rewardId, int points)
    : DomainEvent;

public sealed record GamificationRewardRemovedEvent(Guid userId, Guid rewardId)
    : DomainEvent;

public sealed record AdoptionApplicationAddedEvent(Guid userId, Guid applicationId)
    : DomainEvent;

public sealed record AdoptionApplicationRemovedEvent(Guid userId, Guid applicationId)
    : DomainEvent;

public sealed record AnimalAidRequestAddedEvent(Guid userId, Guid requestId)
    : DomainEvent;

public sealed record AnimalAidRequestRemovedEvent(Guid userId, Guid requestId)
    : DomainEvent;

public sealed record ArticleAddedEvent(Guid userId, Guid articleId)
    : DomainEvent;

public sealed record ArticleRemovedEvent(Guid userId, Guid articleId)
    : DomainEvent;

public sealed record ArticleCommentAddedEvent(Guid userId, Guid commentId)
    : DomainEvent;

public sealed record ArticleCommentRemovedEvent(Guid userId, Guid commentId)
    : DomainEvent;

public sealed record NotificationAddedEvent(Guid userId, Guid notificationId)
    : DomainEvent;

public sealed record NotificationRemovedEvent(Guid userId, Guid notificationId)
    : DomainEvent;

public sealed record SuccessStoryAddedEvent(Guid userId, Guid storyId)
    : DomainEvent;

public sealed record SuccessStoryRemovedEvent(Guid userId, Guid storyId)
    : DomainEvent;

public sealed record LostPetAddedEvent(Guid userId, Guid lostPetId)
    : DomainEvent;

public sealed record LostPetRemovedEvent(Guid userId, Guid lostPetId)
    : DomainEvent;

public sealed record EventAddedEvent(Guid userId, Guid eventId)
    : DomainEvent;

public sealed record EventRemovedEvent(Guid userId, Guid eventId)
    : DomainEvent;

public sealed record DonationAddedEvent(Guid userId, Guid donationId)
    : DomainEvent;

public sealed record DonationRemovedEvent(Guid userId, Guid donationId)
    : DomainEvent;

public sealed record UserLastLoginSetEvent(Guid userId, DateTime lastLogin)
    : DomainEvent;
