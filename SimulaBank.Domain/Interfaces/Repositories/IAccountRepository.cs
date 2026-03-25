using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetByUserId(Guid userId);
        Task<Guid> Create(Guid userId, int number, decimal balance, DateTime dateCreate, bool active);
    }
}