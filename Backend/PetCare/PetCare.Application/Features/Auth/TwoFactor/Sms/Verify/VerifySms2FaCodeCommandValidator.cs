namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

using FluentValidation;

/// <summary>
/// Validator for <see cref="VerifySms2FaCodeCommand"/>.
/// </summary>
public sealed class VerifySms2FaCodeCommandValidator : AbstractValidator<VerifySms2FaCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySms2FaCodeCommandValidator"/> class.
    /// Defines validation rules for the SMS 2FA code.
    /// </summary>
    public VerifySms2FaCodeCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код обов'язковий.")
            .Length(6).WithMessage("Код має складатися з 6 цифр.");
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий.");
    }
}
