namespace PetCare.Application.Features.AdoptionApplications.ChangeStatus;

using FluentValidation;
using PetCare.Domain.Enums;

/// <summary>
/// Validator for <see cref="ChangeAdoptionApplicationStatusCommand"/>.
/// </summary>
public sealed class ChangeAdoptionApplicationStatusCommandValidator
    : AbstractValidator<ChangeAdoptionApplicationStatusCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeAdoptionApplicationStatusCommandValidator"/> class.
    /// </summary>
    public ChangeAdoptionApplicationStatusCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор заявки є обов'язковим.");

        this.RuleFor(x => x.AdminId)
            .NotEmpty()
            .WithMessage("Ідентифікатор адміністратора є обов'язковим.");

        this.RuleFor(x => x.Status)
            .Must(s => s == AdoptionStatus.Approved || s == AdoptionStatus.Rejected)
            .WithMessage("Дозволено лише статуси Approved або Rejected.");

        this.RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .When(x => x.Status == AdoptionStatus.Rejected)
            .WithMessage("Причина відхилення є обов'язковою.");

        this.RuleFor(x => x.RejectionReason)
            .MaximumLength(2000)
            .When(x => x.RejectionReason is not null)
            .WithMessage("Причина відхилення не може перевищувати 2000 символів.");
    }
}
