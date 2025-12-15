namespace PetCare.Application.Features.AdoptionApplications.Reject;

using FluentValidation;

/// <summary>
/// Validator for <see cref="RejectAdoptionApplicationCommand"/>.
/// </summary>
public sealed class RejectAdoptionApplicationCommandValidator
    : AbstractValidator<RejectAdoptionApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RejectAdoptionApplicationCommandValidator"/> class.
    /// </summary>
    public RejectAdoptionApplicationCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор заявки обов'язковий.");

        this.RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Причина відхилення обов'язкова.")
            .MaximumLength(1000)
            .WithMessage("Причина відхилення не може перевищувати 1000 символів.");
    }
}
