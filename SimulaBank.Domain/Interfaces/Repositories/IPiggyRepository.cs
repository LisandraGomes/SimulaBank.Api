using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IPiggyRepository
    {
        Task<List<Piggy>> GetAllByUserId(Guid id, bool onlyActive);
        Task<Piggy> GetById(Guid id);
        Task<Guid> Create(string title, string description, decimal currentValue, int status, DateTime createDate, DateTime dueDate, int dayAutoDeduct, decimal valueAutoDeduct, bool ActiveAutoDeduct, bool active, Guid userId);
        Task Inative(Guid id);
    }
}
