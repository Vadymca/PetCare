namespace PetCare.Application.Features.Auth.ResendVerification;

using FluentValidation;

/// <summary>
/// Provides validation rules for <see cref="ResendVerificationCommand"/>.
/// </summary>
public sealed class ResendVerificationCommandValidator : AbstractValidator<ResendVerificationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResendVerificationCommandValidator"/> class.
    /// Defines validation rules for the email field.
    /// </summary>
    public ResendVerificationCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Невірний формат email.");
    }
}
