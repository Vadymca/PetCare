namespace PetCare.Application.Features.Animals.SubscribeToAnimal;

using FluentValidation;

/// <summary>
/// Validates the <see cref="SubscribeToAnimalCommand"/> to ensure that required fields are provided and valid.
/// </summary>
public class SubscribeToAnimalCommandValidator : AbstractValidator<SubscribeToAnimalCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeToAnimalCommandValidator"/> class.
    /// </summary>
    public SubscribeToAnimalCommandValidator()
    {
        this.RuleFor(x => x.AnimalId)
            .NotEmpty().WithMessage("Ідентифікатор тварини не може бути порожнім.");

        this.RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Ідентифікатор користувача не може бути порожнім.");
    }
}
