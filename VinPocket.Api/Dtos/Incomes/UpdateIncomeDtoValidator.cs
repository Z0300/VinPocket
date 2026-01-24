using FluentValidation;
using VinPocket.Api.Dtos.Incomes;

namespace VinPocket.Api.Dtos.Budgets;

public class UpdateIncomeDtoValidator : AbstractValidator<UpdateIncomeDto>
{
    public UpdateIncomeDtoValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Category ID must not be empty");

        RuleFor(x => x.Amount)
          .NotEmpty()
          .WithMessage("Amount must not be empty")
          .GreaterThanOrEqualTo(0)
          .WithMessage("Amount must be greater than or equal to 0");

        RuleFor(x => x.Source)
           .IsInEnum()
           .WithMessage("Persion must be a valid enum value");

    }
}
