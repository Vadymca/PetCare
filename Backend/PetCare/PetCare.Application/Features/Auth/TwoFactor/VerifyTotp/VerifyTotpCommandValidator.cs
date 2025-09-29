namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;

using FluentValidation;

/// <summary>
/// Validator for <see cref="VerifyTotpCommand"/>.
/// </summary>
public sealed class VerifyTotpCommandValidator : AbstractValidator<VerifyTotpCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpCommandValidator"/> class.
    /// Defines the validation rules for <see cref="VerifyTotpCommand"/>.
    /// </summary>
    public VerifyTotpCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код обов'язковий.")
            .Length(6).WithMessage("Код має складатися з 6 цифр.");
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий.");
    }
}
