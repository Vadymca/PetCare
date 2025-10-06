namespace PetCare.Application.Features.Animals.UnsubscribeFromAnimal;

using FluentValidation;

/// <summary>
/// Validates the <see cref="UnsubscribeFromAnimalCommand"/>.
/// </summary>
public class UnsubscribeFromAnimalCommandValidator : AbstractValidator<UnsubscribeFromAnimalCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeFromAnimalCommandValidator"/> class.
    /// </summary>
    public UnsubscribeFromAnimalCommandValidator()
    {
        this.RuleFor(x => x.AnimalId)
            .NotEmpty().WithMessage("Ідентифікатор тварини не може бути порожнім.");

        this.RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Ідентифікатор користувача не може бути порожнім.");
    }
}
