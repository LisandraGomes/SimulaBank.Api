using Dapper;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;

namespace SimulaBank.Data.Repositories
{
    public class PatternEmailRepository : IPatternEmailRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatternEmailRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PatternEmail> GetById(int id)
        {
            var sql = @"SELECT Subject, Body FROM [PatternEmail] WHERE Id = @Id";

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<PatternEmail>(sql, new { id } , transaction: _unitOfWork.Transaction);
        }
    }
}
