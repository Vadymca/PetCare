namespace PetCare.Application.Features.Auth.ForgotPassword;

using FluentValidation;

/// <summary>
/// Validator for <see cref="ForgotPasswordCommand"/>.
/// </summary>
public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordCommandValidator"/> class.
    /// </summary>
    public ForgotPasswordCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не може бути пустим.")
            .EmailAddress().WithMessage("Email має бути валідним.");
    }
}
