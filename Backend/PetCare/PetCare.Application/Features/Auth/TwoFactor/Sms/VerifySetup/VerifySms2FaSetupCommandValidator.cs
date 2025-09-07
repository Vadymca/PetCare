namespace PetCare.Application.Features.Auth.TwoFactor.Sms.VerifySetup;

using FluentValidation;

/// <summary>
/// Validator for <see cref="VerifySms2FaSetupCommand"/>.
/// </summary>
public sealed class VerifySms2FaSetupCommandValidator : AbstractValidator<VerifySms2FaSetupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySms2FaSetupCommandValidator"/> class.
    /// </summary>
    public VerifySms2FaSetupCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код обов'язковий.")
            .Length(6).WithMessage("Код має складатися з 6 цифр.");
    }
}
