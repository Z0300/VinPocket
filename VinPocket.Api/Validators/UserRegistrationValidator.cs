using FluentValidation;
using VinPocket.Api.Contracts.Users;

namespace VinPocket.Api.Validators;

public sealed class UserRegistrationValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
