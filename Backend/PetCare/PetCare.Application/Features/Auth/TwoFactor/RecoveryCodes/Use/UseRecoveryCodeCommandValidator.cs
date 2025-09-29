namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UseRecoveryCodeCommand"/>.
/// </summary>
public sealed class UseRecoveryCodeCommandValidator : AbstractValidator<UseRecoveryCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UseRecoveryCodeCommandValidator"/> class.
    /// Validator for <see cref="UseRecoveryCodeCommand"/>.
    /// Ensures that the recovery code is provided and matches the expected format.
    /// </summary>
    public UseRecoveryCodeCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код відновлення обов'язковий.")
            .Matches(@"^[A-Z0-9]{5}-[A-Z0-9]{5}$").WithMessage("Невірний формат резервного коду. Повинно бути у вигляді XXXXX-XXXXX.");
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий.");
    }
}
