namespace PetCare.Application.Features.AnimalAidRequests.Create;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="CreateAnimalAidRequestCommand"/> ensuring all required fields are correctly populated and formatted.
/// </summary>
public sealed class CreateAnimalAidRequestCommandValidator : AbstractValidator<CreateAnimalAidRequestCommand>
{
     /// <summary>
    /// Initializes a new instance of the <see cref="CreateAnimalAidRequestCommandValidator"/> class
    /// and defines validation rules for creating a new animal aid request.
    /// </summary>
    public CreateAnimalAidRequestCommandValidator()
    {
        this.RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва запиту обов'язкова.")
            .MaximumLength(200).WithMessage("Назва не може перевищувати 200 символів.");

        this.RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Короткий опис обов'язковий.");

        this.RuleFor(x => x.Description)
            .MaximumLength(4000).WithMessage("Опис не може перевищувати 4000 символів.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        this.RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Очікувана сума допомоги не може бути від'ємною.")
            .When(x => x.EstimatedCost.HasValue);

        this.RuleForEach(x => x.Photos)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Усі URL фотографій мають бути валідними.");
    }
}
