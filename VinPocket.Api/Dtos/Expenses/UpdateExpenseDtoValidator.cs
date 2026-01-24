using FluentValidation;

namespace VinPocket.Api.Dtos.Expenses;

public class UpdateExpenseDtoValidator : AbstractValidator<UpdateExpenseDto>
{
    public UpdateExpenseDtoValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Category ID must not be empty");

        RuleFor(x => x.Amount)
          .NotEmpty()
          .WithMessage("Amount must not be empty")
          .GreaterThanOrEqualTo(0)
          .WithMessage("Amount must be greater than or equal to 0");

        RuleFor(x => x.Payment)
           .IsInEnum()
           .WithMessage("Persion must be a valid enum value");

        RuleFor(x => x.ExpenseDate)
          .NotEmpty()
          .WithMessage("Date must not be empty")
          .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
          .WithMessage("Date can not be in the future");
    }
}
