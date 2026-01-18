using FluentValidation;
using VinPocket.Api.Contracts.Users;

namespace VinPocket.Api.Validators;

public sealed class UserLoginValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
