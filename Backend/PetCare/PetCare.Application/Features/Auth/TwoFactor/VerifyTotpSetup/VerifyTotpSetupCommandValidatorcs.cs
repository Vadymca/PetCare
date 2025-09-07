namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpSetup;

using FluentValidation;

/// <summary>
/// Validator for <see cref="VerifyTotpSetupCommand"/>.
/// </summary>
public sealed class VerifyTotpSetupCommandValidator : AbstractValidator<VerifyTotpSetupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpSetupCommandValidator"/> class.
    /// Sets up the validation rules for email and TOTP code.
    /// </summary>
    public VerifyTotpSetupCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код обов'язковий.")
            .Length(6).WithMessage("Код повинен містити 6 символів.");
    }
}
