using SimulaBank.Application.Input;
using SimulaBank.Application.Outputs;

namespace SimulaBank.Application.Interfaces
{
    public interface ITransactionApplication
    {
        Task<PatternResult<TransactionOutput>> Create(TransactionInput input);
    }
}
