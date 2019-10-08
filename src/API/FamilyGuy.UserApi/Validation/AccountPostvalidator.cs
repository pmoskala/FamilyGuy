using FamilyGuy.UserApi.Model;
using FluentValidation;
using FluentValidation.Validators;

namespace FamilyGuy.UserApi.Validation
{
    public class AccountPostValidator : AbstractValidator<AccountPostModel>
    {
        public AccountPostValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
            RuleFor(p => p.Email)
                .MinimumLength(5)
                .EmailAddress(EmailValidationMode.Net4xRegex);
            RuleFor(p => p.LoginName)
                .MinimumLength(5);
            RuleFor(p => p.Name)
                .MinimumLength(3);
            RuleFor(p => p.Surname)
                .MinimumLength(3);
            RuleFor(p => p.Password)
                .MinimumLength(8);
            RuleFor(p => p.TelephoneNumber)
                .MinimumLength(9);
        }
    }
}
