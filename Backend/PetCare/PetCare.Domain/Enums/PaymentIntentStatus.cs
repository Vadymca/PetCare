namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a payment intent.
/// </summary>
public enum PaymentIntentStatus
{
    /// <summary>
    /// The payment intent has been created but not yet completed.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The payment intent has been successfully completed.
    /// </summary>
    Succeeded = 1,

    /// <summary>
    /// The payment intent has failed.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// The payment intent has been canceled and will not be processed.
    /// </summary>
    Canceled = 3,
}
