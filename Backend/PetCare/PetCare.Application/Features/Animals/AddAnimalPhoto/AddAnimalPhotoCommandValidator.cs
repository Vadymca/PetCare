namespace PetCare.Application.Features.Animals.AddAnimalPhoto;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="AddAnimalPhotoCommand"/>.
/// </summary>
public class AddAnimalPhotoCommandValidator : AbstractValidator<AddAnimalPhotoCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddAnimalPhotoCommandValidator"/> class.
    public AddAnimalPhotoCommandValidator()
    {
        this.RuleFor(x => x.AnimalId)
            .NotEmpty()
            .WithMessage("Id тварини не може бути порожнім.");

        this.RuleFor(x => x.PhotoUrl)
            .NotEmpty()
            .WithMessage("URL фото не може бути порожнім.")
            .Must(this.BeAValidUrl)
            .WithMessage("URL фото має бути дійсним HTTP/HTTPS посиланням.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
