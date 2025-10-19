namespace PetCare.Application.Features.Species.UpdateSpecie;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateSpecieCommand"/>.
/// Ensures that the species name is valid.
/// </summary>
public sealed class UpdateSpecieCommandValidator : AbstractValidator<UpdateSpecieCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSpecieCommandValidator"/> class, configuring validation rules for updating.
    /// a species.
    /// </summary>
    /// <remarks>This validator enforces that the species identifier (Id) is not empty and that the species
    /// name (Name) is required and does not exceed 100 characters. Use this validator to ensure that update commands
    /// for species meet the required data integrity constraints before processing.</remarks>
    public UpdateSpecieCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id виду не може бути пустим.");

        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва виду не може бути пустою.")
            .MaximumLength(100).WithMessage("Назва виду не може перевищувати 100 символів.");
    }
}
