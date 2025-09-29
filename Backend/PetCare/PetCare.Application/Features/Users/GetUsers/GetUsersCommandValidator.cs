namespace PetCare.Application.Features.Users.GetUsers;

using FluentValidation;

/// <summary>
/// Validator for <see cref="GetUsersCommand"/>.
/// Ensures that pagination parameters are valid.
/// </summary>
internal class GetUsersCommandValidator : AbstractValidator<GetUsersCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersCommandValidator"/> class.
    /// Defines rules for page number and page size.
    /// </summary>
    public GetUsersCommandValidator()
    {
        this.RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки має бути більшим за 0.");

        this.RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100.");
    }
}