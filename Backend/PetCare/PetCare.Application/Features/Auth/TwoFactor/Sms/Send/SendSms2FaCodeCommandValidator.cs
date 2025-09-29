namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Send;

using FluentValidation;

/// <summary>
/// Validator for <see cref="SendSms2FaCodeCommand"/>.
/// </summary>>
public sealed class SendSms2FaCodeCommandValidator : AbstractValidator<SendSms2FaCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendSms2FaCodeCommandValidator"/> class.
    /// Defines the validation rules for <see cref="SendSms2FaCodeCommand"/>.
    /// </summary>
    public SendSms2FaCodeCommandValidator()
    {
        this.RuleFor(x => x.TwoFaToken)
            .NotEmpty().WithMessage("Токен 2FA обов'язковий.");
    }
}
