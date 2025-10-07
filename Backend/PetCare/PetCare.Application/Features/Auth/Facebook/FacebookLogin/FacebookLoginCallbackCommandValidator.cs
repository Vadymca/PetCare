namespace PetCare.Application.Features.Auth.Facebook.FacebookLogin;

using FluentValidation;

/// <summary>
/// Validator for <see cref="FacebookLoginCallbackCommand"/>.
/// </summary>
public sealed class FacebookLoginCallbackCommandValidator : AbstractValidator<FacebookLoginCallbackCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FacebookLoginCallbackCommandValidator"/> class.
    /// </summary>
    public FacebookLoginCallbackCommandValidator()
    {
        this.RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Код авторизації обов’язковий.");

        this.RuleFor(x => x.State)
            .NotEmpty().WithMessage("State параметр обов’язковий.");
    }
}
