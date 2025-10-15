namespace PetCare.Application.Features.Animals.RemoveAnimalPhoto;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="RemoveAnimalPhotoCommand"/>.
/// </summary>
public class RemoveAnimalPhotoCommandValidator : AbstractValidator<RemoveAnimalPhotoCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveAnimalPhotoCommandValidator"/> class.
    /// </summary>
    public RemoveAnimalPhotoCommandValidator()
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
