namespace PetCare.Application.Features.Species.CreateSpecie;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateSpecieCommand"/>.
/// Ensures the species name is not null, empty, or too long.
/// </summary>
public sealed class CreateSpecieCommandValidator : AbstractValidator<CreateSpecieCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSpecieCommandValidator"/> class, configuring validation rules for the Name.
    /// property.
    /// </summary>
    /// <remarks>The validator enforces that the Name property is required and must not exceed 100 characters.
    /// Use this validator to ensure that species creation commands meet these constraints before processing.</remarks>
    public CreateSpecieCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва виду не може бути пустою.")
            .MaximumLength(100).WithMessage("Назва виду не може перевищувати 100 символів.");
    }
}
