using FluentValidation;

namespace VinPocket.Api.Dtos.Users;

public sealed class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationRequestDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
           .NotEmpty();
    }
}
