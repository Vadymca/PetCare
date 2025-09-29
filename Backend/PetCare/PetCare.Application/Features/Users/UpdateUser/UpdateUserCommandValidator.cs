namespace PetCare.Application.Features.Users.UpdateUser;

using FluentValidation;

/// <summary>
/// Validator for UpdateUserCommand.
/// </summary>
public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandValidator"/> class.
    /// Defines validation rules for <see cref="UpdateUserCommand"/>.
    /// </summary>
    public UpdateUserCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id користувача не може бути пустим.");

        this.RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email має бути валідною електронною адресою.");

        this.RuleFor(x => x.Password)
           .MinimumLength(8).WithMessage("Пароль має містити щонайменше 8 символів.")
           .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).+$")
           .WithMessage("Пароль має містити принаймні одну велику літеру, одну малу літеру, одну цифру та один спеціальний символ.")
           .When(x => !string.IsNullOrWhiteSpace(x.Password));

        this.RuleFor(x => x.FirstName)
          .MaximumLength(50).WithMessage("Ім'я не може перевищувати 50 символів.")
          .Matches(@"^[a-zA-Zа-яА-ЯіІїЇєЄ''\s-]+$").WithMessage("Ім'я може містити тільки літери, апострофи, пробіли та дефіси.");

        this.RuleFor(x => x.LastName)
            .MaximumLength(50).WithMessage("Прізвище не може перевищувати 50 символів.")
            .Matches(@"^[a-zA-Zа-яА-ЯіІїЇєЄ''\s-]+$").WithMessage("Прізвище може містити тільки літери, апострофи, пробіли та дефіси.");

        this.RuleFor(x => x.Phone)
          .Matches(@"^\+\d{8,15}$")
          .When(x => !string.IsNullOrWhiteSpace(x.Phone))
          .WithMessage("Номер телефону має бути у міжнародному форматі, наприклад +380XXXXXXXXX.");

        this.RuleFor(x => x.Preferences)
           .Must(prefs => prefs == null || prefs.Count <= 50)
           .WithMessage("Кількість налаштувань не може перевищувати 50.");

        this.RuleForEach(x => x.Preferences)
            .Must(kv => kv.Key.Length <= 50 && kv.Value.Length <= 200)
            .WithMessage("Ключ не може бути довшим за 50 символів, значення — за 200.");

        this.RuleFor(x => x.Points)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Points.HasValue)
            .WithMessage("Points має бути невід'ємним числом.");

        this.RuleFor(x => x.ProfilePhoto)
           .MaximumLength(500)
           .When(x => !string.IsNullOrWhiteSpace(x.ProfilePhoto))
           .WithMessage("ProfilePhoto не може бути довшим за 500 символів.");

        this.RuleFor(x => x.Language)
           .Matches(@"^[a-zA-Z\-]{2,5}$")
           .When(x => !string.IsNullOrWhiteSpace(x.Language))
           .WithMessage("Language має бути у форматі ISO (наприклад, 'en', 'uk').");

        this.RuleFor(x => x.PostalCode)
          .MaximumLength(20)
          .When(x => !string.IsNullOrWhiteSpace(x.PostalCode))
          .WithMessage("Поштовий індекс не може бути довшим за 20 символів.");
    }
}
