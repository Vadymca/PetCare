namespace PetCare.Domain.Enums;

/// <summary>
/// Specifies the scope of a subscription within the system.
/// </summary>
/// <remarks>Use this enumeration to indicate whether a subscription applies globally or is limited to a specific
/// context, such as an aid request or guardianship. The scope determines how and where the subscription is
/// applied.</remarks>
public enum SubscriptionScope
{
    /// <summary>
    /// Specifies that the member applies globally, without restriction to a specific scope or context.
    /// </summary>
    Global = 0,

    /// <summary>
    /// Represents an aid request status or type within the enumeration.
    /// </summary>
    AidRequest = 1,

    /// <summary>
    /// Represents a guardianship relationship or status within the enumeration.
    /// </summary>
    Guardianship = 2,
}
