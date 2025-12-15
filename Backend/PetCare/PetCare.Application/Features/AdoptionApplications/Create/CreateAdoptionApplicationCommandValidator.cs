namespace PetCare.Application.Features.AdoptionApplications.Create;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateAdoptionApplicationCommand"/>.
/// </summary>
public sealed class CreateAdoptionApplicationCommandValidator
    : AbstractValidator<CreateAdoptionApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAdoptionApplicationCommandValidator"/> class.
    /// </summary>
    public CreateAdoptionApplicationCommandValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Ідентифікатор користувача є обов'язковим.");

        this.RuleFor(x => x.AnimalId)
            .NotEmpty()
            .WithMessage("Ідентифікатор тварини є обов'язковим.");

        this.RuleFor(x => x.Comment)
            .MaximumLength(2000)
            .WithMessage("Коментар не може перевищувати 2000 символів.")
            .When(x => x.Comment is not null);
    }
}
