using FluentValidation;

namespace VinPocket.Api.Dtos.Users;

public sealed class UserLoginDtoValidator : AbstractValidator<UserLoginRequestDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
