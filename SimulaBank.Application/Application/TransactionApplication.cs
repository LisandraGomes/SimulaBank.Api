using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.DomainServices;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Messages;
using System.Net;

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
            if (input == null)
                return new PatternResult<TransactionOutput>(HttpStatusCode.UnprocessableEntity, ResultMessages.UnprocessableEntity);

            if (input.UserId == Guid.Empty)
                return new PatternResult<TransactionOutput>(HttpStatusCode.UnprocessableEntity, ResultMessages.UnprocessableEntity);

            var validTransaction = TransactionDomainService.Validate(input.Value, input.DateCreate, input.DateFinally, input.TypeId, input.IdAccountDestination, input.IdAccountOrigin);

            var idCreated = await _transactionRepository.CreateTransaction(input.Value, input.TypeId, input.DateFinally, input.IdAccountOrigin, input.IdAccountDestination);
            return new PatternResult<TransactionOutput>(new TransactionOutput { Id = idCreated });
        }
    }
}
