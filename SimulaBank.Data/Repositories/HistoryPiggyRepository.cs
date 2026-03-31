using Dapper;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;

namespace SimulaBank.Data.Repositories
{
    public class HistoryPiggyRepository : IHistoryPiggyRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public HistoryPiggyRepository(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public async Task Create(string idPiggy, int typeHistory, decimal value, DateTime date)
        {
            var sql = @"INSERT INTO HistoryPiggy (PiggyId, TypeHistoryId ,CurrenteValue, ValueTransaction, CreateDate, TransactionDate, UserCreate)
                        VALUES (@IdPiggy, @Type, @Value, @ValueTransaction, @Date, @TransactionDate, @UserCreate)";

            var parameters = new DynamicParameters();
            parameters.Add("@IdPiggy", idPiggy);
            parameters.Add("@Type", typeHistory);
            parameters.Add("@Value", value);
            parameters.Add("@ValueTransaction", value);
            parameters.Add("@Date", DateTime.Now);
            parameters.Add("@TransactionDate", date);
            parameters.Add("@UserCreate", string.Empty);

            await _unitOfWork.Connection.ExecuteAsync(sql, parameters, transaction: _unitOfWork.Transaction);

        }

        public async Task<List<HistoryPiggy>> GetAllByPiggyId(Guid piggyId)
        {
            var sql = @"SELECT Id, PiggyId, TypeHisotryId ,CurrenteValue, ValueTransaction, CreateDate, TransactionDate, UserCreate FROM HistoryPiggy WHERE PiggyId = @PiggyId";

            var result = await _unitOfWork.Connection.QueryAsync<HistoryPiggy>(sql, new { PiggyId = piggyId }, transaction: _unitOfWork.Transaction);
            return result.ToList();
        }



    }
}