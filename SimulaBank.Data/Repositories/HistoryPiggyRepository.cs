using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;

namespace SimulaBank.Data.Repositories
{
    public class HistoryPiggyRepository : IHistoryPiggyRepository
    {
        private readonly string _connectionString;
        public HistoryPiggyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<List<HistoryPiggy>> GetAllByPiggyId(Guid piggyId)
        {
            var sql = @"SELECT Id, PiggyId, TypeHisotryId ,CurrenteValue, ValueTransaction, CreateDate, TransactionDate, UserCreate FROM HistoryPiggy WHERE PiggyId = @PiggyId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<HistoryPiggy>(sql, new { PiggyId = piggyId });
                return result.ToList();
            }
        }



    }
}