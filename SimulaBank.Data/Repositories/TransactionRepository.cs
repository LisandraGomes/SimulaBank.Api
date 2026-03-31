using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Enum;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;

namespace SimulaBank.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateTransaction(decimal value, ETypeTransaction typeTransaction, DateTime? dateFinally, string? idAccountOrigin, string? idAccountDestination)
        {
            var sql = @"INSERT INTO [Transaction]
                        (@Value, @TypeTransaction, @IdAccountOrigin, @IdAccountDestination, @Date, @DateFinally, @Active );
                        SELECT SCOPE_IDENTITY();";


            _unitOfWork.BeginTransaction();
            try
            {
                var idTransaction = await _unitOfWork.Connection.ExecuteScalarAsync<Guid>(sql, new
                {
                    Value = value,
                    TypeTransaction = typeTransaction,
                    IdAccountOrigin = idAccountOrigin,
                    IdAccountDestination = idAccountDestination,
                    Date = DateTime.UtcNow,
                    DateFinally = dateFinally,
                    Active = true
                }, transaction: _unitOfWork.Transaction);
                _unitOfWork.Commit();
                return idTransaction;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateTransaction(Guid idTransaction, DateTime dateFinally)
        {
            var sql = @"UPDATE [Transaction]
                        SET DateFinally = @DateFinally
                        WHERE Id = @Id";



            var rowsAffected = await _unitOfWork.Connection.ExecuteAsync(sql, new
            {
                Id = idTransaction,
                DateFinally = dateFinally
            }, transaction: _unitOfWork.Transaction);
            _unitOfWork.Commit();
            return rowsAffected > 0;

        }


        public async Task<List<Transaction>> GetAllTransactionByUser(Guid idUser)
        {
            var sql = @"SELECT t.Id, t.Value, t.TypeTransaction, t.IdAccountOrigin, t.IdAccountDestination, t.Date, t.DateFinally, t.Active, t.IdUser
                        FROM [Transaction] t
                        INNER JOIN Account a ON (t.IdAccountOrigin = a.Id OR t.IdAccountDestination = a.Id)
                        WHERE a.IdUser = @IdUser AND t";

            var transactions = await _unitOfWork.Connection.QueryAsync<Transaction>(sql, new { IdUser = idUser }, transaction: _unitOfWork.Transaction);
            return transactions.ToList();
        }
    }

}