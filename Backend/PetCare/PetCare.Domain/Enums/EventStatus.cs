namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of an event.
/// </summary>
public enum EventStatus
{
    /// <summary>
    /// The event is planned but has not yet started.
    /// </summary>
    Planned,

    /// <summary>
    /// The event is currently ongoing.
    /// </summary>
    Ongoing,

    /// <summary>
    /// The event has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The event has been cancelled.
    /// </summary>
    Cancelled,
}
