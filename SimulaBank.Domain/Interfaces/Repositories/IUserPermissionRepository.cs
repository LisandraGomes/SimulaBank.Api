using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IUserPermissionRepository
    {
        Task<List<Permission>> GetPermissions(Guid userId);
    }
}
