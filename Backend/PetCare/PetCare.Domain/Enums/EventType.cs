namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the types of events supported by the system.
/// </summary>
public enum EventType
{
    /// <summary>
    /// An event focused on pet adoption.
    /// </summary>
    AdoptionDay,

    /// <summary>
    /// An event aimed at raising funds.
    /// </summary>
    Fundraiser,

    /// <summary>
    /// An online seminar or webinar event.
    /// </summary>
    Webinar,

    /// <summary>
    /// Training event for volunteers.
    /// </summary>
    VolunteerTraining,
}
