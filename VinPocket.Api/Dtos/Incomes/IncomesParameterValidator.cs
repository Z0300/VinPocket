using FluentValidation;

namespace VinPocket.Api.Dtos.Incomes;

public sealed class IncomesParameterValidator : AbstractValidator<IncomesParameters>
{
    public IncomesParameterValidator()
    {
        RuleFor(x => x.Page)
         .GreaterThan(0)
         .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
         .InclusiveBetween(1, 50)
         .WithMessage("Page size must be between 1 and 50");
    }
}
