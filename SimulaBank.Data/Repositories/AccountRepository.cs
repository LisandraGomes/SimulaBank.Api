using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using System.Data;

namespace SimulaBank.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;
        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Guid> Create(Guid userId, int number, decimal balance, DateTime dateCreate, bool active)
        {
            var sql = @"INSERT INTO [Account] (UserId, AccountNumber, Balance, DateCreate, Active)
                        OUTPUT INSERTED.Id
                        VALUES(@UserId, @Number, @Balance, @DateCreate, @Active)";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Guid);
            parameters.Add("@Number", number, DbType.Int32);
            parameters.Add("@Balance", balance, DbType.Decimal);
            parameters.Add("@DateCreate", dateCreate, DbType.DateTime);
            parameters.Add("@Active", active, DbType.Boolean);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<Guid>(sql, parameters);
            }
        }

        public async Task<Account> GetByUserId(Guid UserId) {
            var sql = @"SELECT Id, AccountNumber, Balance,DateCreate, Active FROM [Account] WHERE UserId = @UserId";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { UserId = UserId });
            }
            
        }

    }
}
