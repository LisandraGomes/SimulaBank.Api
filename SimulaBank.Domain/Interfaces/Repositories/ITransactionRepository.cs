using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Enum;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<Guid> CreateTransaction(decimal value, ETypeTransaction typeTransaction, DateTime? dateFinally, string? idAccountOrigin, string? idAccountDestination);
        Task<List<Transaction>> GetAllTransactionByUser(Guid idUser);
        Task<bool> UpdateTransaction(Guid idTransaction, DateTime dateFinally);
    }
}
