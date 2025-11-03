namespace PetCare.Application.Features.PaymentMethods.CreatePaymentMethod;

using FluentValidation;

/// <summary>
/// Validates <see cref="CreatePaymentMethodCommand"/> input data.
/// </summary>
public sealed class CreatePaymentMethodCommandValidator : AbstractValidator<CreatePaymentMethodCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePaymentMethodCommandValidator"/> class, configuring validation rules for.
    /// the Name property of a payment method creation command.
    /// </summary>
    /// <remarks>The validator enforces that the Name property is required, must be between 3 and 50
    /// characters in length, and may only contain letters, spaces, or hyphens. These rules help ensure that payment
    /// method names are valid and conform to expected formatting standards.</remarks>
    public CreatePaymentMethodCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва методу оплати є обов’язковою.")
            .MinimumLength(3).WithMessage("Назва методу оплати повинна містити щонайменше 3 символи.")
            .MaximumLength(50).WithMessage("Назва методу оплати не може перевищувати 50 символів.")
            .Matches(@"^[A-Za-zА-Яа-яІіЇїЄє\s\-]+$")
                .WithMessage("Назва методу оплати може містити лише літери, пробіли або дефіси.");
    }
}
