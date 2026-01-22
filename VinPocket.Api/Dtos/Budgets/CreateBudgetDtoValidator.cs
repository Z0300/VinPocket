using FluentValidation;

namespace VinPocket.Api.Dtos.Budgets;

public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDto>
{
    public CreateBudgetDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID must not be empty");

        RuleFor(x => x.Amount)
          .NotEmpty()
          .WithMessage("Amount must not be empty")
          .GreaterThanOrEqualTo(0)
          .WithMessage("Amount must be greater than or equal to 0");

        RuleFor(x => x.StartDate)
           .NotEmpty()
           .WithMessage("Date must not be empty")
           .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
           .WithMessage("Date can not be in the future");

    }
}
