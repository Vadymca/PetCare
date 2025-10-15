namespace PetCare.Application.Features.Users.Roles;

using System;
using FluentValidation;

/// <summary>
/// Validator for the <see cref="AddUserRoleCommand"/> command.
/// Ensures that the UserId and Role values are valid before the command is handled.
/// </summary>
public sealed class AddUserRoleCommandValidator : AbstractValidator<AddUserRoleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserRoleCommandValidator"/> class.
    /// Defines validation rules for UserId and Role.
    /// </summary>
    public AddUserRoleCommandValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");

        this.RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role є обов'язковою.")
            .Must(role => Enum.TryParse(typeof(Domain.Enums.UserRole), role, true, out _))
            .WithMessage("Role має бути дійсною роллю користувача.");
    }
}
