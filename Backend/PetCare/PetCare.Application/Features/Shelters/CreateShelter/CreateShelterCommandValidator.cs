namespace PetCare.Application.Features.Shelters.CreateShelter;

using System;
using System.Linq;
using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateShelterCommand"/>.
/// </summary>
public sealed class CreateShelterCommandValidator : AbstractValidator<CreateShelterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateShelterCommandValidator"/> class.
    /// </summary>
    public CreateShelterCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва притулку не може бути порожньою.")
            .MaximumLength(100).WithMessage("Назва притулку не може перевищувати 100 символів.");

        this.RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Адреса не може бути порожньою.");

        this.RuleFor(x => x.ContactPhone)
            .NotEmpty().WithMessage("Контактний номер не може бути порожнім.");

        this.RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Контактний email не може бути порожнім.")
            .EmailAddress().WithMessage("Невірний формат email.");

        this.RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Місткість повинна бути більшою за 0.");

        this.RuleFor(x => x.Photos)
            .Must(list => list == null || list.All(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)))
            .WithMessage("Усі URL фотографій мають бути валідними.");
    }
}
