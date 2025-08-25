namespace PetCare.Domain.Enums;

/// <summary>
/// Defines categories of aid that can be requested or provided.
/// </summary>
public enum AidCategory
{
    /// <summary>
    /// Aid related to food supplies.
    /// </summary>
    Food,

    /// <summary>
    /// Aid related to medical needs or treatments.
    /// </summary>
    Medical,

    /// <summary>
    /// Aid involving equipment or physical resources.
    /// </summary>
    Equipment,

    /// <summary>
    /// Aid that does not fit into predefined categories.
    /// </summary>
    Other,
}
