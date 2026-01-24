using FluentValidation;

namespace VinPocket.Api.Dtos.Expenses;

public sealed class ExpensesParametersValidator : AbstractValidator<ExpensesParameters>
{
    public ExpensesParametersValidator()
    {
        RuleFor(x => x.Page)
        .GreaterThan(0)
        .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
         .InclusiveBetween(1, 50)
         .WithMessage("Page size must be between 1 and 50");
    }
}
