using FluentValidation;

namespace VinPocket.Api.Dtos.Auth;

public sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
