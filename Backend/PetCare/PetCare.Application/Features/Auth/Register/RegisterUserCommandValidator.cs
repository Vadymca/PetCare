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
        this.RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Електронна пошта є обов'язковою.")
            .EmailAddress().WithMessage("Невірний формат електронної пошти.")
            .MaximumLength(255).WithMessage("Електронна пошта не може перевищувати 255 символів.");

        this.RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.")
            .MinimumLength(8).WithMessage("Пароль має містити щонайменше 8 символів.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Пароль має містити принаймні одну велику літеру, одну малу літеру, одну цифру та один спеціальний символ.");

        this.RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ім'я є обов'язковим.")
            .MaximumLength(50).WithMessage("Ім'я не може перевищувати 50 символів.")
            .Matches(@"^[a-zA-Zа-яА-ЯіІїЇєЄ''\s-]+$").WithMessage("Ім'я може містити тільки літери, апострофи, пробіли та дефіси.");

        this.RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище є обов'язковим.")
            .MaximumLength(50).WithMessage("Прізвище не може перевищувати 50 символів.")
            .Matches(@"^[a-zA-Zа-яА-ЯіІїЇєЄ''\s-]+$").WithMessage("Прізвище може містити тільки літери, апострофи, пробіли та дефіси.");

        this.RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Номер телефону є обов'язковим.")
            .Matches(@"^\+\d{8,15}$").WithMessage("Номер телефону має бути у міжнародному форматі, наприклад +380XXXXXXXXX.");
    }
}
