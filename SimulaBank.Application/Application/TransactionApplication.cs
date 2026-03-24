using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.Interfaces.Repositories;

namespace SimulaBank.Application.Application
{
    public class TransactionApplication : ITransactionApplication
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionApplication(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PatternResult<TransactionOutput>> Create(TransactionInput input)
        {
            var idCreated = await _transactionRepository.CreateTransaction(input.Value, input.TypeId, input.DateFinally, input.IdAccountOrigin, input.IdAccountDestination);
            return new PatternResult<TransactionOutput>(new TransactionOutput { Id = idCreated });
        }
    }
}
