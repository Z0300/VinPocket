using FluentValidation;

namespace VinPocket.Api.Dtos.Auth;

public sealed class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
