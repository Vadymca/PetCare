namespace PetCare.Domain.Enums;

/// <summary>
/// Specifies the status of a subscription, indicating whether it is active, canceled, or paused.
/// </summary>
/// <remarks>Use this enumeration to represent and check the current state of a subscription in business logic or
/// user interfaces. The values correspond to typical lifecycle states for subscriptions, such as ongoing service,
/// cancellation, or temporary suspension.</remarks>
public enum SubscriptionStatus
{
    /// <summary>
    /// Indicates that the item is currently active.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Indicates that the operation was canceled before completion.
    /// </summary>
    Canceled = 1,

    /// <summary>
    /// Indicates that the operation or process is currently paused.
    /// </summary>
    Paused = 2,
}
