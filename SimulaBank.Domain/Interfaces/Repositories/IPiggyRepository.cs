using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IPiggyRepository
    {
        Task<List<Piggy>> GetAllPiggyByUserId(Guid id);
        Task<Piggy> GetPiggyById(Guid id);
        Task CreatePiggy(string title, string description, decimal currentValue, int status, DateTime createDate, DateTime dueDate, int dayAutoDeduct, decimal valueAutoDeduct, bool ActiveAutoDeduct, bool active, Guid userId);
    }
}
