using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByLogin(string email, string cpf);
        Task<bool> CheckThePassword(string cpf, string passwordHash);
        Task<Guid> InsertUser(string firstName, string midName, string cpf, string email, string passwordHash, DateTime birthDate, int idRole);
        Task<bool> ActiveUser(Guid userId, string email);
    }
}
