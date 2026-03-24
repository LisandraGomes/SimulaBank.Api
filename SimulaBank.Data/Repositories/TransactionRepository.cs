using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Enum;
using SimulaBank.Domain.Interfaces.Repositories;

namespace SimulaBank.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;
        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Guid> CreateTransaction(decimal value, ETypeTransaction typeTransaction, DateTime? dateFinally, string? idAccountOrigin, string? idAccountDestination)
        {
            var sql = @"INSERT INTO [Transaction]
                        (@Value, @TypeTransaction, @IdAccountOrigin, @IdAccountDestination, @Date, @DateFinally, @Active );
                        SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var idTransaction = await connection.ExecuteScalarAsync<Guid>(sql, new
                        {
                            Value = value,
                            TypeTransaction = typeTransaction,
                            IdAccountOrigin = idAccountOrigin,
                            IdAccountDestination = idAccountDestination,
                            Date = DateTime.UtcNow,
                            DateFinally = dateFinally,
                            Active = true
                        }, transaction);
                        transaction.Commit();
                        return idTransaction;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateTransaction(Guid idTransaction, DateTime dateFinally)
        {
            var sql = @"UPDATE [Transaction]
                        SET DateFinally = @DateFinally
                        WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var rowsAffected = await connection.ExecuteAsync(sql, new
                        {
                            Id = idTransaction,
                            DateFinally = dateFinally
                        }, transaction);
                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<List<Transaction>> GetAllTransactionByUser(Guid idUser)
        {
            var sql = @"SELECT t.Id, t.Value, t.TypeTransaction, t.IdAccountOrigin, t.IdAccountDestination, t.Date, t.DateFinally, t.Active, t.IdUser
                        FROM [Transaction] t
                        INNER JOIN Account a ON (t.IdAccountOrigin = a.Id OR t.IdAccountDestination = a.Id)
                        WHERE a.IdUser = @IdUser AND t";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var transactions = await connection.QueryAsync<Transaction>(sql, new { IdUser = idUser });
                return transactions.ToList();
            }
        }


    }
}
