namespace PetCare.Domain.ValueObjects;

public sealed class TotpSetupInfo
{
    public string QrCodeImage { get; init; }

    public string ManualKey { get; init; }

    public string[] RecoveryCodes { get; init; }

    public TotpSetupInfo(string qrCodeImage, string manualKey, string[] recoveryCodes)
    {
        this.QrCodeImage = qrCodeImage;
        this.ManualKey = manualKey;
        this.RecoveryCodes = recoveryCodes;
    }
}
