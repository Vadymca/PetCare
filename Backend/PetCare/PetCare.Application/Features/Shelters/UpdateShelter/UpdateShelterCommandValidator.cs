namespace PetCare.Application.Features.Shelters.UpdateShelter;

using System;
using FluentValidation;

/// <summary>
/// Validates the <see cref="UpdateShelterCommand"/> to ensure all required fields are correctly populated and formatted.
/// </summary>
public sealed class UpdateShelterCommandValidator : AbstractValidator<UpdateShelterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateShelterCommandValidator"/> class, defining validation rules for updating.
    /// shelter information.
    /// </summary>
    /// <remarks>This validator enforces constraints on the Id, Name, Address, and Photos properties of an
    /// update command. It ensures that the shelter identifier is provided, the name and address do not exceed their
    /// maximum lengths if specified, and that all photo URLs are valid absolute URIs.</remarks>
    public UpdateShelterCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ідентифікатор притулку обов'язковий.");

        this.RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.")
            .When(x => x.Name is not null);

        this.RuleFor(x => x.Address)
            .MaximumLength(300).WithMessage("Адреса не може перевищувати 300 символів.")
            .When(x => x.Address is not null);

        this.RuleForEach(x => x.Photos)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Усі URL фотографій мають бути валідними.");
    }
}
