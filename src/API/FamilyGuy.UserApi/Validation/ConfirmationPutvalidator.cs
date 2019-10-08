using FamilyGuy.UserApi.Model;
using FluentValidation;

namespace FamilyGuy.UserApi.Validation
{
    public class ConfirmationPutValidator : AbstractValidator<ConfirmationPutModel>
    {
        public ConfirmationPutValidator()
        {
            RuleFor(p => p.Confirmed)
                .NotNull();
        }
    }
}
