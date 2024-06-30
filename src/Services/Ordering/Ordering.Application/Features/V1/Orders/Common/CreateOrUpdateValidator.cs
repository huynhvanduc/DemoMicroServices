using FluentValidation;
using Ordering.Application.Features.V1.Orders;

namespace Ordering.Application.Features.V1.Orders;

public class CreateOrUpdateValidator : AbstractValidator<CreateOrUpdateCommand>
{
    public CreateOrUpdateValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("{FirstName} is required.")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("{FirstName} must not exceed 50 characteres.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("{LastName} is required.")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("{LastName} must not exceed 50 characteres.");

        RuleFor(p => p.EmailAddress)
            .EmailAddress().WithMessage("{EmailAddress} is invalid format")
            .NotEmpty().WithMessage("{EmailAddress} is required.");

        RuleFor(p => p.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
    }
}
