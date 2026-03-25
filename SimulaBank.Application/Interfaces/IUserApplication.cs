using SimulaBank.Application.Input;
using SimulaBank.Application.Outputs;

namespace SimulaBank.Application.Interfaces
{
    public interface IUserApplication
    {
        Task<PatternResult> Register(RegisterUserInput request);
        Task<PatternResult> Active(ActiveUserInput input);
    }
}
