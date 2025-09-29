namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

using FluentValidation;

/// <summary>
/// Validator for <see cref="VerifyTotpBackupCodeCommand"/>.
/// </summary>
public sealed class VerifyTotpBackupCodeCommandValidator : AbstractValidator<VerifyTotpBackupCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpBackupCodeCommandValidator"/> class.
    /// Defines validation rules for the backup code.
    /// </summary>
    public VerifyTotpBackupCodeCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Резервний код обов'язковий.")
            .Matches(@"^[A-Z0-9]{5}-[A-Z0-9]{5}$").WithMessage("Невірний формат резервного коду. Повинно бути у вигляді XXXXX-XXXXX.");
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий");
    }
}
