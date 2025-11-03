namespace PetCare.Application.Features.PaymentMethods.UpdatePaymentMethod;

using FluentValidation;

/// <summary>
/// Validates <see cref="UpdatePaymentMethodCommand"/> input data.
/// </summary>
public sealed class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentMethodCommandValidator"/> class.
    /// Provides validation rules for updating a payment method, ensuring that required fields are present and conform
    /// to expected formats.
    /// </summary>
    /// <remarks>This validator enforces that the payment method identifier is specified and that the new name
    /// meets length and character requirements. The new name must be between 3 and 50 characters and contain only
    /// letters, spaces, or hyphens. Use this validator to verify input data before processing an update to a payment
    /// method.</remarks>
    public UpdatePaymentMethodCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ідентифікатор методу оплати є обов’язковим.");

        this.RuleFor(x => x.NewName)
            .NotEmpty().WithMessage("Нова назва методу оплати є обов’язковою.")
            .MinimumLength(3).WithMessage("Назва повинна містити щонайменше 3 символи.")
            .MaximumLength(50).WithMessage("Назва не може перевищувати 50 символів.")
            .Matches(@"^[A-Za-zА-Яа-яІіЇїЄє\s\-]+$")
                .WithMessage("Назва може містити лише літери, пробіли або дефіси.");
    }
}
