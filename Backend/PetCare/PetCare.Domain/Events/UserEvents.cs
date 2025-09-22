namespace PetCare.Domain.Events;
public sealed record UserCreatedEvent(Guid UserId)
    : DomainEvent;

public sealed record UserEmailConfirmedEvent(Guid UserId, string Email)
    : DomainEvent;

public sealed record UserProfileUpdatedEvent(Guid UserId)
    : DomainEvent;

public sealed record UserProfilePhotoChangedEvent(Guid UserId, string? NewPhotoUrl)
    : DomainEvent;

public sealed record UserProfilePhotoRemovedEvent(Guid UserId, string OldPhotoUrl)
    : DomainEvent;

public sealed record UserPointsAddedEvent(Guid UserId, int Amount)
    : DomainEvent;

public sealed record UserPointsDeductedEvent(Guid UserId, int Amount)
    : DomainEvent;

public sealed record UserPasswordChangedEvent(Guid UserId)
    : DomainEvent;

public sealed record ShelterSubscriptionAddedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

public sealed record ShelterSubscriptionUpdatedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

public sealed record ShelterSubscriptionRemovedEvent(Guid UserId, Guid ShelterId)
    : DomainEvent;

public sealed record GamificationRewardAddedEvent(Guid UserId, Guid RewardId, int Points)
    : DomainEvent;

public sealed record GamificationRewardRemovedEvent(Guid UserId, Guid RewardId)
    : DomainEvent;

public sealed record AdoptionApplicationAddedEvent(Guid UserId, Guid ApplicationId)
    : DomainEvent;

public sealed record AdoptionApplicationRemovedEvent(Guid UserId, Guid ApplicationId)
    : DomainEvent;

public sealed record AnimalAidRequestAddedEvent(Guid UserId, Guid RequestId)
    : DomainEvent;

public sealed record AnimalAidRequestRemovedEvent(Guid UserId, Guid RequestId)
    : DomainEvent;

public sealed record ArticleAddedEvent(Guid UserId, Guid ArticleId)
    : DomainEvent;

public sealed record ArticleRemovedEvent(Guid UserId, Guid ArticleId)
    : DomainEvent;

public sealed record ArticleCommentAddedEvent(Guid UserId, Guid CommentId)
    : DomainEvent;

public sealed record ArticleCommentRemovedEvent(Guid UserId, Guid CommentId)
    : DomainEvent;

public sealed record NotificationAddedEvent(Guid UserId, Guid NotificationId)
    : DomainEvent;

public sealed record NotificationRemovedEvent(Guid UserId, Guid NotificationId)
    : DomainEvent;

public sealed record SuccessStoryAddedEvent(Guid UserId, Guid StoryId)
    : DomainEvent;

public sealed record SuccessStoryRemovedEvent(Guid UserId, Guid StoryId)
    : DomainEvent;

public sealed record LostPetAddedEvent(Guid UserId, Guid LostPetId)
    : DomainEvent;

public sealed record LostPetRemovedEvent(Guid UserId, Guid LostPetId)
    : DomainEvent;

public sealed record EventAddedEvent(Guid UserId, Guid EventId)
    : DomainEvent;

public sealed record EventRemovedEvent(Guid UserId, Guid EventId)
    : DomainEvent;

public sealed record DonationAddedEvent(Guid UserId, Guid DonationId)
    : DomainEvent;

public sealed record DonationRemovedEvent(Guid UserId, Guid DonationId)
    : DomainEvent;

public sealed record UserLastLoginSetEvent(Guid UserId, DateTime LastLogin)
    : DomainEvent;
