namespace PetCare.Api.Endpoints.Payments.LiqPay;

using MediatR;
using PetCare.Application.Features.Payments.LiqPay.LiqPayCallback;

/// <summary>
/// Handles incoming LiqPay payment callback requests.
/// </summary>
public static class LiqPayCallbackEndpoint
{
    /// <summary>
    /// Maps the POST /api/payments/liqpay/callback endpoint used by LiqPay to send payment status notifications.
    /// </summary>
    /// <param name="app">The web application instance used to configure routes.</param>
    public static void MapLiqPayCallbackEndpoint(this WebApplication app)
    {
        app.MapPost("/api/payments/liqpay/callback", async (
            HttpRequest request,
            IMediator mediator,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("LiqPayCallbackEndpoint");

            try
            {
                // Read form values
                var form = await request.ReadFormAsync();
                var data = form["data"].ToString();
                var signature = form["signature"].ToString();

                // Validate presence
                if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(signature))
                {
                    logger.LogWarning("LiqPay callback: missing 'data' or 'signature'.");
                    return Results.BadRequest("Missing required LiqPay fields.");
                }

                logger.LogInformation("Received LiqPay callback. Data length: {Length}", data.Length);

                // Decode Base64 for debug
                try
                {
                    var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));
                    logger.LogInformation("Decoded LiqPay JSON: {DecodedJson}", decoded);
                }
                catch (Exception decodeEx)
                {
                    logger.LogError(decodeEx, "Failed to decode Base64 LiqPay data. Raw input: {RawData}", data);
                    return Results.Problem($"Base64 decode error: {decodeEx.Message}");
                }

                // Process via MediatR
                var command = new HandleLiqPayCallbackCommand(data, signature);
                var result = await mediator.Send(command);

                logger.LogInformation("LiqPay callback processed successfully.");
                return result;
            }
            catch (Exception ex)
            {
                // Return full error for debugging in Scalar
                logger.LogError(ex, "Error while handling LiqPay callback.");
                return Results.Problem($"Callback error: {ex}");
            }
        })
        .WithName("LiqPayCallback")
        .WithTags("Payments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireRateLimiting("GlobalPolicy");
    }
}
