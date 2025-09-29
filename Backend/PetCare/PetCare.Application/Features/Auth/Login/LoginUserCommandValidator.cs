namespace PetCare.Application.Features.Auth.Login;

using FluentValidation;

/// <summary>
/// Validator for the <see cref="LoginUserCommand"/> request.
/// Ensures that email and password fields are provided and valid.
/// </summary>
public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginUserCommandValidator"/> class.
    /// Defines validation rules for <see cref="LoginUserCommand"/>.
    /// </summary>
    public LoginUserCommandValidator()
    {
        this.RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Електронна пошта є обов'язковою.")
           .EmailAddress().WithMessage("Невірний формат електронної пошти.");

        this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.");
    }
}
