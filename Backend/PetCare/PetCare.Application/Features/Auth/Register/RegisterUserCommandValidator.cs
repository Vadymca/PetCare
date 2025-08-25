namespace PetCare.Application.Features.Auth.Register;
using FluentValidation;

/// <summary>
/// Validator for <see cref="RegisterUserCommand"/>.
/// </summary>
public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserCommandValidator"/> class.
    /// Defines validation rules for user registration.
    /// </summary>
    public RegisterUserCommandValidator()
    {
        this.RuleFor(x => x.email)
            .NotEmpty().WithMessage("Електронна пошта є обов’язковою.")
            .EmailAddress().WithMessage("Невірний формат електронної пошти.");

        this.RuleFor(x => x.password)
            .NotEmpty().WithMessage("Пароль є обов’язковим.")
            .MinimumLength(6).WithMessage("Пароль має містити щонайменше 6 символів.");

        this.RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("Ім’я є обов’язковим.");

        this.RuleFor(x => x.lastName)
            .NotEmpty().WithMessage("Прізвище є обов’язковим.");

        this.RuleFor(x => x.phoneNumber)
            .NotEmpty().WithMessage("Номер телефону є обов’язковим.")
            .Matches(@"^\+?[1-9]\d{7,14}$").WithMessage("Невірний формат номера телефону.");
    }
}
