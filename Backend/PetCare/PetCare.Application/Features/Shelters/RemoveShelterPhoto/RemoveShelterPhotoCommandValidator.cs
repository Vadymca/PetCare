namespace PetCare.Application.Features.Shelters.RemoveShelterPhoto;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="RemoveShelterPhotoCommand"/>.
/// </summary>
public sealed class RemoveShelterPhotoCommandValidator : AbstractValidator<RemoveShelterPhotoCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveShelterPhotoCommandValidator"/> class.
    /// </summary>
    public RemoveShelterPhotoCommandValidator()
    {
        this.RuleFor(x => x.ShelterId)
            .NotEmpty()
            .WithMessage("Id притулку не може бути порожнім.");

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
