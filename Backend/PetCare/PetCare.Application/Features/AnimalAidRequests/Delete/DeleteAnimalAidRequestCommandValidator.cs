namespace PetCare.Application.Features.AnimalAidRequests.Delete;

using FluentValidation;

/// <summary>
/// Validator for <see cref="DeleteAnimalAidRequestCommand"/>.
/// </summary>
public sealed class DeleteAnimalAidRequestCommandValidator
    : AbstractValidator<DeleteAnimalAidRequestCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAnimalAidRequestCommandValidator"/> class.
    /// </summary>
    public DeleteAnimalAidRequestCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор запиту на допомогу є обов'язковим.");
    }
}
