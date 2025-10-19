namespace PetCare.Application.Features.Shelters.SubscribeToShelter;

using FluentValidation;

/// <summary>
/// Validates the <see cref="SubscribeToShelterCommand"/> to ensure that required fields are provided and valid.
/// </summary>
public sealed class SubscribeToShelterCommandValidator : AbstractValidator<SubscribeToShelterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeToShelterCommandValidator"/> class.
    /// </summary>
    public SubscribeToShelterCommandValidator()
    {
        this.RuleFor(x => x.ShelterId)
            .NotEmpty()
            .WithMessage("Ідентифікатор притулку не може бути порожнім.");

        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Ідентифікатор користувача не може бути порожнім.");
    }
}
