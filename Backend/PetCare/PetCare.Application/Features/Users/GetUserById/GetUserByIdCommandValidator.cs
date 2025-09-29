namespace PetCare.Application.Features.Users.GetUserById;

using FluentValidation;

/// <summary>
/// Validator for <see cref="GetUserByIdCommand"/>.
/// Ensures that the command contains valid data.
/// </summary>
internal class GetUserByIdCommandValidator : AbstractValidator<GetUserByIdCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdCommandValidator"/> class.
    /// </summary>
    public GetUserByIdCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id користувача не може бути пустим.");
    }
}
