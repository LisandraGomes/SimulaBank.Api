using SimulaBank.Domain.Entities;

namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IPatternEmailRepository
    {
        Task<PatternEmail> GetById(int id);
    }
}
