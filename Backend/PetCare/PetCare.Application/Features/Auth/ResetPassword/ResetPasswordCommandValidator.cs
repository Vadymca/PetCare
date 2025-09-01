namespace PetCare.Application.Features.Auth.ResetPassword;

using FluentValidation;

/// <summary>
/// Validator for ResetPasswordCommand.
/// </summary>
public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordCommandValidator"/> class.
    /// Defines validation rules for email, token, and new password.
    /// </summary>
    public ResetPasswordCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не може бути порожнім.")
            .EmailAddress().WithMessage("Email має бути валідним.");

        this.RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Токен не може бути порожнім.");

        this.RuleFor(x => x.NewPassword)
           .NotEmpty().WithMessage("Пароль є обов'язковим.")
           .MinimumLength(8).WithMessage("Пароль має містити щонайменше 8 символів.")
           .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
           .WithMessage("Пароль має містити принаймні одну велику літеру, одну малу літеру, одну цифру та один спеціальний символ.");
    }
}
