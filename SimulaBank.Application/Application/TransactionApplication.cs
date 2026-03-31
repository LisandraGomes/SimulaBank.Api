using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.DomainServices;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using SimulaBank.Domain.Messages;
using System.Net;

namespace SimulaBank.Application.Application
{
    public class TransactionApplication : ITransactionApplication
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionApplication(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PatternResult<TransactionOutput>> Create(TransactionInput input)
        {
            try
            {
                if (input == null)
                    return new PatternResult<TransactionOutput>(HttpStatusCode.UnprocessableEntity, ResultMessages.UnprocessableEntity);

                if (input.UserId == Guid.Empty)
                    return new PatternResult<TransactionOutput>(HttpStatusCode.UnprocessableEntity, ResultMessages.UnprocessableEntity);

                var validTransaction = TransactionDomainService.Validate(input.Value, input.DateCreate, input.DateFinally, input.TypeId, input.IdAccountDestination, input.IdAccountOrigin);

                if (validTransaction.IsValid)
                {
                    var transaction = await FinallyTransaction(input);
                    if (transaction != null && transaction.Id != Guid.Empty)
                        return new PatternResult<TransactionOutput>(transaction);

                    return new PatternResult<TransactionOutput>(HttpStatusCode.BadRequest, ResultMessages.InternalError);
                }
                return new PatternResult<TransactionOutput>(HttpStatusCode.UnprocessableEntity, validTransaction.ErrorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new PatternResult<TransactionOutput>(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

        private async Task<TransactionOutput> FinallyTransaction(TransactionInput input)
        {
            try
            {
                var user = await _userRepository.GetUserById(input.UserId);
                if (user.Id != Guid.Empty && user.IdAccount != Guid.Empty)
                {
                    _unitOfWork.BeginTransaction();
                    var addValueToAccount = await _accountRepository.AddValueTransaction(input.Value, user.IdAccount);
                    var idCreated = await _transactionRepository.CreateTransaction(input.Value, input.TypeId, input.DateFinally, input.IdAccountOrigin, input.IdAccountDestination);
                    _unitOfWork.Commit();

                    return new TransactionOutput { Id = idCreated };
                }
                return new TransactionOutput { Id = Guid.Empty };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _unitOfWork.Rollback();
                return new TransactionOutput { Id = Guid.Empty };
            }
        }

        public async Task<PatternResult<List<TransactionOutput>>> GetByUser(Guid idUser)
        {
            try
            {
                //var transactions = await _transactionRepository.GetByUser(idUser);
                //if (transactions != null)
                //    return new PatternResult<List<TransactionOutput>>(transactions);
                return new PatternResult<List<TransactionOutput>>(HttpStatusCode.NotFound, ResultMessages.UserNotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new PatternResult<List<TransactionOutput>>(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

    }
}
