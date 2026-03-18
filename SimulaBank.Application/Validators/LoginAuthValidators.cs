using FluentValidation;
using SimulaBank.Application.Input;

namespace SimulaBank.Application.Validators
{
    public class LoginAuthValidators : AbstractValidator<AuthUserInput>
    {
        public LoginAuthValidators()
        {
            RuleFor(x => x.Login).NotEmpty().MinimumLength(11).NotEqual("00000000000");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
