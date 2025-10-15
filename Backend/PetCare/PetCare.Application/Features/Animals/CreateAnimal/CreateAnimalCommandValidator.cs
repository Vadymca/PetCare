namespace PetCare.Application.Features.Animals.CreateAnimal;

using System;
using System.Linq;
using FluentValidation;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Validator for <see cref="CreateAnimalCommand"/>.
/// </summary>
public sealed class CreateAnimalCommandValidator : AbstractValidator<CreateAnimalCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAnimalCommandValidator"/> class.
    /// </summary>
    public CreateAnimalCommandValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Ідентифікатор користувача не може бути порожнім.");

        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім'я тварини не може бути порожнім.")
            .MaximumLength(100).WithMessage("Ім'я тварини не може перевищувати 100 символів.");

        this.RuleFor(x => x.BreedId)
            .NotEmpty().WithMessage("Ідентифікатор породи не може бути порожнім.");

        this.RuleFor(x => x.ShelterId)
            .NotEmpty().WithMessage("Ідентифікатор притулку не може бути порожнім.");

        this.RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Розмір тварини має бути дійсним значенням.");

        this.RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Стать тварини має бути дійсним значенням.");

        this.RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Статус тварини має бути дійсним значенням.");

        this.RuleFor(x => x.CareCost)
            .IsInEnum().WithMessage("Вартість догляду тварини має бути дійсним значенням.");

        this.RuleFor(x => x.MicrochipId)
             .Must(microchip => string.IsNullOrWhiteSpace(microchip) || MicrochipId.IsValid(microchip))
             .WithMessage("Неправильний формат ідентифікатора мікрочіпа. Має бути 5-20 символів: A-Z, 0-9.");

        this.RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Вага має бути більшою за 0.")
            .When(x => x.Weight.HasValue);

        this.RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Зріст має бути більшим за 0.")
            .When(x => x.Height.HasValue);

        this.RuleFor(x => x.Photos)
            .Must(list => list == null || list.All(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)))
            .WithMessage("Всі URL фотографій повинні бути валідними.");

        this.RuleFor(x => x.HealthConditions)
            .Must(list => list == null || list.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Усі записи про стан здоров'я повинні бути не порожніми.");

        this.RuleFor(x => x.SpecialNeeds)
            .Must(list => list == null || list.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Усі записи про спеціальні потреби повинні бути не порожніми.");

        this.RuleFor(x => x.Temperaments)
            .Must(list => list == null || list.All(t => Enum.IsDefined(typeof(AnimalTemperament), t)))
            .WithMessage("Усі темпераменти повинні бути дійсними.");
    }
}
