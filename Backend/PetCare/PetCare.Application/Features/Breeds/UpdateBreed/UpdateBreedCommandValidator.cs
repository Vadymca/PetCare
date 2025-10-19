namespace PetCare.Application.Features.Breeds.UpdateBreed;

using FluentValidation;

/// <summary>
/// Validates the <see cref="UpdateBreedCommand"/> for updating a breed.
/// </summary>
public sealed class UpdateBreedCommandValidator : AbstractValidator<UpdateBreedCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBreedCommandValidator"/> class, which defines validation rules for updating.
    /// breed information.
    /// </summary>
    /// <remarks>This validator enforces that the breed identifier is required, the name does not exceed 100
    /// characters if provided, and the description does not exceed 500 characters if present. Use this validator to
    /// ensure that update commands for breeds meet the expected data constraints before processing.</remarks>
    public UpdateBreedCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id породи обов'язковий.");

        this.RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Ім'я породи не може бути довше 100 символів.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        this.RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Опис не може бути довше 500 символів.")
            .When(x => x.Description is not null);
    }
}
