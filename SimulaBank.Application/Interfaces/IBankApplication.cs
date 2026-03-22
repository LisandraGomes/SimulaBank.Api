using SimulaBank.Application.Input;
using SimulaBank.Application.Outputs;

namespace SimulaBank.Application.Interfaces
{
    public interface IBankApplication
    {
        Task<PatternResult> CreatePiggy(PiggyRegisterInput request, string cpf);
        Task<PatternResult<List<PiggyOutput>>> GetAllPiggyByUserId(Guid userId);
    }
}
