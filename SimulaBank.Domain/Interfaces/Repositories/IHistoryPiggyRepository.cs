using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IHistoryPiggyRepository
    {
        Task Create(string idPiggy, int typeHistory, decimal value, DateTime date);
        Task<List<HistoryPiggy>> GetAllByPiggyId(Guid piggyId);
    }
}
