using FamilyGuy.UserApi.Model;
using FluentValidation;

namespace FamilyGuy.UserApi.Validation
{
    public class UserAuthenticationPostValidator : AbstractValidator<UserAuthenticationPostModel>
    {
        public UserAuthenticationPostValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password con not be empty");
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("User name can not be empty.");
        }
    }
}
