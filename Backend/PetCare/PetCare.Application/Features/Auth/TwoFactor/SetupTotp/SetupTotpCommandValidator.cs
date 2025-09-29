namespace PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

using FluentValidation;

/// <summary>
/// Validator for <see cref="SetupTotpCommand"/>.
/// </summary>
public sealed class SetupTotpCommandValidator : AbstractValidator<SetupTotpCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupTotpCommandValidator"/> class.
    /// Defines the validation rules for <see cref="SetupTotpCommand"/>.
    /// </summary>
    public SetupTotpCommandValidator()
    {
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий.");
    }
}
