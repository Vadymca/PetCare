namespace PetCare.Application.Features.Auth.Facebook.GetFacebookLoginUrl;

using System;
using FluentValidation;

/// <summary>
/// Validator for <see cref="GetFacebookLoginUrlCommand"/>.
/// </summary>
public sealed class GetFacebookLoginUrlCommandValidator : AbstractValidator<GetFacebookLoginUrlCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetFacebookLoginUrlCommandValidator"/> class.
    /// </summary>
    public GetFacebookLoginUrlCommandValidator()
    {
        this.RuleFor(x => x.State)
            .NotEmpty().WithMessage("Поле State не може бути порожнім")
            .Must(s => Guid.TryParse(s, out _))
            .WithMessage("Поле State має бути дійсним GUID");
    }
}
