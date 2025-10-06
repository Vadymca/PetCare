namespace PetCare.Application.Features.Animals.UpdateAnimal;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateAnimalCommand"/>.
/// </summary>
public sealed class UpdateAnimalCommandValidator : AbstractValidator<UpdateAnimalCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalCommandValidator"/> class.
    /// </summary>
    public UpdateAnimalCommandValidator()
    {
        // Id обов’язковий (беремо з маршруту)
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ідентифікатор тварини є обов'язковим.");

        // Name — якщо передане, то має бути валідне
        this.RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.")
            .When(x => x.Name is not null);

        // Description
        this.RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Опис не може перевищувати 1000 символів.")
            .When(x => x.Description is not null);

        // Adoption requirements
        this.RuleFor(x => x.AdoptionRequirements)
            .MaximumLength(500).WithMessage("Вимоги до адопції не можуть перевищувати 500 символів.")
            .When(x => x.AdoptionRequirements is not null);

        // MicrochipId — якщо передано, то не пусте
        this.RuleFor(x => x.MicrochipId)
            .MaximumLength(64).WithMessage("Мікрочіп Id не може перевищувати 64 символів.")
            .When(x => x.MicrochipId is not null);

        // Вага
        this.RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Вага повинна бути більшою за 0.")
            .When(x => x.Weight.HasValue);

        // Висота
        this.RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Висота повинна бути більшою за 0.")
            .When(x => x.Height.HasValue);

        // Колекції
        this.RuleForEach(x => x.HealthConditions)
            .MaximumLength(100).WithMessage("Кожен запис про стан здоров’я не може перевищувати 100 символів.");

        this.RuleForEach(x => x.SpecialNeeds)
            .MaximumLength(100).WithMessage("Кожна спеціальна потреба не може перевищувати 100 символів.");
    }
}
