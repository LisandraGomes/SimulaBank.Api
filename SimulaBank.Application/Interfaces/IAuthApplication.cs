using SimulaBank.Application.Input;
using SimulaBank.Application.Outputs;

namespace SimulaBank.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<PatternResult<AuthOutput>> Login(AuthUserInput auth);
    }
}
