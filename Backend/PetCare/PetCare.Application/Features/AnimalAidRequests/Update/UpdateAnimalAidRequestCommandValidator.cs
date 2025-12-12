namespace PetCare.Application.Features.AnimalAidRequests.Update;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateAnimalAidRequestCommand"/>.
/// Ensures provided values are valid.
/// </summary>
public sealed class UpdateAnimalAidRequestCommandValidator : AbstractValidator<UpdateAnimalAidRequestCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalAidRequestCommandValidator"/> class.
    /// </summary>
    public UpdateAnimalAidRequestCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ідентифікатор запиту обов'язковий.");

        this.RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Назва не може перевищувати 200 символів.")
            .When(x => x.Title is not null);

        this.RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("Короткий опис не може перевищувати 500 символів.")
            .When(x => x.ShortDescription is not null);

        this.RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0).WithMessage("Вартість не може бути від’ємною.")
            .When(x => x.EstimatedCost.HasValue);

        this.RuleFor(x => x.Photos)
            .Must(list => list == null || list.TrueForAll(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)))
            .WithMessage("Усі URL фотографій мають бути валідними.");
    }
}
