
using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Services
{
    public interface IAuthServices
    {
        string ComputeHash(string password);
        string GenerateToken(Guid id, string userEmail, string userCpf, bool emailAutorized, string role, string roleDescription, List<Permission> permissions);
    }
}
