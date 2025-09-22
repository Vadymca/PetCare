namespace PetCare.Infrastructure.Services.Sms;

public class SmsFlySettings
{
    public string ApiKey { get; set; } = string.Empty;

    public string Sender { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://sms-fly.ua/api/v2/api.php";
}
