using FluentValidation;

namespace VinPocket.Api.Dtos.Categories;

public sealed class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
