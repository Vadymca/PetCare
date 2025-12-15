namespace PetCare.Application.Features.AdoptionApplications.Update;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateAdoptionApplicationCommand"/>.
/// </summary>
public sealed class UpdateAdoptionApplicationCommandValidator
    : AbstractValidator<UpdateAdoptionApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAdoptionApplicationCommandValidator"/> class.
    /// </summary>
    public UpdateAdoptionApplicationCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор заявки є обов'язковим.");

        this.RuleFor(x => x.Comment)
            .MaximumLength(2000)
            .WithMessage("Коментар не може перевищувати 2000 символів.")
            .When(x => x.Comment is not null);

        this.RuleFor(x => x.AdminNotes)
            .MaximumLength(3000)
            .WithMessage("Адміністративні нотатки не можуть перевищувати 3000 символів.")
            .When(x => x.AdminNotes is not null);
    }
}
