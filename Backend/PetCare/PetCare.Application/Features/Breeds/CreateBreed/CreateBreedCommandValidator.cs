namespace PetCare.Application.Features.Breeds.CreateBreed;

using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateBreedCommand"/> to ensure required fields and constraints are met.
/// </summary>
public sealed class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBreedCommandValidator"/> class with validation rules for creating a breed.
    /// </summary>
    /// <remarks>The validator enforces that the Name property is required and cannot exceed 100 characters,
    /// the SpecieId property is required, and the Description property cannot exceed 500 characters if provided. These
    /// rules help ensure that breed creation requests contain valid and properly formatted data.</remarks>
    public CreateBreedCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім'я породи обов'язкове.")
            .MaximumLength(100).WithMessage("Ім'я породи не може бути довше 100 символів.");

        this.RuleFor(x => x.SpecieId)
            .NotEmpty().WithMessage("Id виду обов'язковий.");

        this.RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Опис не може бути довше 500 символів.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
