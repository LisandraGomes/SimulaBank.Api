using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByLogin(string email, string password);
        Task<bool> CheckThePassword(string cpf, string passwordHash);
    }
}
