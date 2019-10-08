using FamilyGuy.UserApi.Model;
using FluentValidation;

namespace FamilyGuy.UserApi.Validation
{
    public class PasswordChangePutValidator : AbstractValidator<PasswordChangePutModel>
    {
        public PasswordChangePutValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty()
                .WithMessage("User Id is required.");
            
            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .WithMessage("Password can not be empty.")
                .MinimumLength(8)
                .WithMessage("Password minimum length is 8 characters.");
        }
    }
}