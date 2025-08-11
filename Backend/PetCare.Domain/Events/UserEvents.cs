namespace PetCare.Domain.Events;

public sealed record UserCreatedEvent(Guid userId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserProfileUpdatedEvent(Guid userId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserProfilePhotoChangedEvent(Guid userId, string? newPhotoUrl)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserProfilePhotoRemovedEvent(Guid userId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserPointsAddedEvent(Guid userId, int amount)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserPointsDeductedEvent(Guid userId, int amount)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserPasswordChangedEvent(Guid userId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterSubscriptionAddedEvent(Guid userId, Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterSubscriptionUpdatedEvent(Guid userId, Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ShelterSubscriptionRemovedEvent(Guid userId, Guid shelterId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record GamificationRewardAddedEvent(Guid userId, Guid rewardId, int points)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record GamificationRewardRemovedEvent(Guid userId, Guid rewardId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AdoptionApplicationAddedEvent(Guid userId, Guid applicationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AdoptionApplicationRemovedEvent(Guid userId, Guid applicationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalAidRequestAddedEvent(Guid userId, Guid requestId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record AnimalAidRequestRemovedEvent(Guid userId, Guid requestId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ArticleAddedEvent(Guid userId, Guid articleId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ArticleRemovedEvent(Guid userId, Guid articleId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ArticleCommentAddedEvent(Guid userId, Guid commentId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record ArticleCommentRemovedEvent(Guid userId, Guid commentId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record NotificationAddedEvent(Guid userId, Guid notificationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record NotificationRemovedEvent(Guid userId, Guid notificationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record SuccessStoryAddedEvent(Guid userId, Guid storyId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record SuccessStoryRemovedEvent(Guid userId, Guid storyId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record LostPetAddedEvent(Guid userId, Guid lostPetId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record LostPetRemovedEvent(Guid userId, Guid lostPetId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record EventAddedEvent(Guid userId, Guid eventId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record EventRemovedEvent(Guid userId, Guid eventId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record DonationAddedEvent(Guid userId, Guid donationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record DonationRemovedEvent(Guid userId, Guid donationId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

public sealed record UserLastLoginSetEvent(Guid userId, DateTime lastLogin)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
