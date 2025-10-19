namespace PetCare.Application.Features.Shelters.UnsubscribeFromShelter;

using FluentValidation;

/// <summary>
/// Provides validation logic for the UnsubscribeFromShelterCommand to ensure required properties are specified.
/// </summary>
/// <remarks>This validator checks that both the shelter identifier and user identifier are present and
/// not empty before processing an unsubscribe request. Use this class to enforce command integrity when handling
/// unsubscribe operations.</remarks>
public class UnsubscribeFromShelterCommandValidator : AbstractValidator<UnsubscribeFromShelterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeFromShelterCommandValidator"/> class, configuring validation rules.
    /// for the unsubscribe command parameters.
    /// </summary>
    /// <remarks>This constructor sets up validation to ensure that both the shelter identifier and
    /// user identifier are provided and not empty when processing an unsubscribe request.</remarks>
    public UnsubscribeFromShelterCommandValidator()
    {
        this.RuleFor(x => x.ShelterId)
            .NotEmpty().WithMessage("Ідентифікатор притулку не може бути порожнім.");
        this.RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Ідентифікатор користувача не може бути порожнім.");
    }
}
