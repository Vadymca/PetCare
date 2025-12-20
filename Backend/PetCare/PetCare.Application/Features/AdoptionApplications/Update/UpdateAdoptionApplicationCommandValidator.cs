namespace PetCare.Application.Features.AdoptionApplications.Update;

using FluentValidation;

/// <summary>
/// Validator for <see cref="UpdateAdoptionApplicationCommand"/>.
/// </summary>
public sealed class UpdateAdoptionApplicationCommandValidator
    : AbstractValidator<UpdateAdoptionApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAdoptionApplicationCommandValidator"/> class.
    /// </summary>
    public UpdateAdoptionApplicationCommandValidator()
    {
        this.RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("Ідентифікатор заявки є обов'язковим.");

        this.RuleFor(x => x.Comment)
            .MaximumLength(2000)
            .WithMessage("Коментар не може перевищувати 2000 символів.")
            .When(x => x.Comment is not null);

        this.RuleFor(x => x.AdminNotes)
            .MaximumLength(3000)
            .WithMessage("Адміністративні нотатки не можуть перевищувати 3000 символів.")
            .When(x => x.AdminNotes is not null);

        this.RuleFor(x => x.CuratorName)
            .MaximumLength(200)
            .WithMessage("Ім'я куратора не може перевищувати 200 символів.")
            .When(x => x.CuratorName is not null);

        this.RuleFor(x => x.CuratorPhone)
            .MaximumLength(50)
            .WithMessage("Телефон куратора не може перевищувати 50 символів.")
            .When(x => x.CuratorPhone is not null);

        this.RuleFor(x => x)
            .Must(x =>
                (x.CuratorName is null && x.CuratorPhone is null) ||
                (!string.IsNullOrWhiteSpace(x.CuratorName) &&
                 !string.IsNullOrWhiteSpace(x.CuratorPhone)))
            .WithMessage("Ім'я та телефон куратора повинні бути заповнені разом.");

        this.RuleFor(x => x.MeetingDate)
            .Must(date => date is null || date.Value > DateTime.UtcNow)
            .WithMessage("Дата зустрічі не може бути в минулому.");

        this.RuleFor(x => x.AdoptionDate)
            .Must(date => date is null || date.Value > DateTime.UtcNow)
            .WithMessage("Дата усиновлення не може бути в минулому.");
    }
}
