namespace PetCare.Application.Dtos.Payments;

using System;

/// <summary>
/// Represents the response data for a LiqPay payment status request, including order details, payment status, and
/// related metadata.
/// </summary>
/// <param name="OrderId">The unique identifier of the order associated with the payment.</param>
/// <param name="Status">The current status of the payment, such as 'success', 'failure', or other LiqPay-defined states.</param>
/// <param name="Action">The type of action performed for the payment, as reported by LiqPay (for example, 'pay', 'hold', or 'refund').</param>
/// <param name="Amount">The total amount of the payment transaction, expressed in the specified currency.</param>
/// <param name="Currency">The ISO currency code representing the currency of the payment (for example, 'UAH', 'USD', 'EUR').</param>
/// <param name="Description">An optional description or comment associated with the payment. May be null if not provided.</param>
/// <param name="CreatedDate">The date and time when the payment was created, or null if not available.</param>
/// <param name="EndDate">The date and time when the payment was completed or finalized, or null if not available.</param>
public sealed record LiqPayPaymentStatusResponseDto(
string OrderId,
string Status,
string Action,
decimal Amount,
string Currency,
string? Description,
DateTime? CreatedDate,
DateTime? EndDate);
