namespace PetCare.Application.Features.Auth.ConfirmEmail;

using FluentValidation;

/// <summary>
/// Validator for <see cref="ConfirmEmailCommand"/>.
/// Ensures that email and token are provided and valid.
/// </summary>
public sealed class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandValidator"/> class.
    /// Defines validation rules for confirming user email.
    /// </summary>
    public ConfirmEmailCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обов'язковий.")
            .EmailAddress().WithMessage("Невірний формат email.");

        this.RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token обов'язковий.");
    }
}
