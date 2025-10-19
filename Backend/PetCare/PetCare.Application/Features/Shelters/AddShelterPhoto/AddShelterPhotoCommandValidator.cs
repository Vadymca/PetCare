namespace PetCare.Application.Features.Shelters.AddShelterPhoto;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="AddShelterPhotoCommand"/>.
/// </summary>
public sealed class AddShelterPhotoCommandValidator : AbstractValidator<AddShelterPhotoCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddShelterPhotoCommandValidator"/> class, configuring validation rules for adding.
    /// a shelter photo command.
    /// </summary>
    /// <remarks>This validator enforces that the shelter ID and photo URL are provided and that the photo URL
    /// is a valid HTTP or HTTPS link. Use this validator to ensure that commands for adding shelter photos meet the
    /// required criteria before processing.</remarks>
    public AddShelterPhotoCommandValidator()
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
