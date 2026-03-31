using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using System.Data;

namespace SimulaBank.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private IUnitOfWork _unitOfWork;
        public AccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return await _unitOfWork.Connection.QuerySingleAsync<Guid>(sql, parameters, _unitOfWork.Transaction);
        }

        public async Task<Account> GetByUserId(Guid userId) {
            var sql = @"SELECT Id, AccountNumber, Balance,DateCreate, Active FROM [Account] WHERE UserId = @UserId";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Account>(sql, new {UserId = userId}, _unitOfWork.Transaction);  
        }

        public async Task<decimal> AddValueTransaction(decimal value, Guid idAccount)
        {
            try
            {
                var parameters = new DynamicParameters();
                decimal balance = 0;
                var select = @"SELECT Balance FROM [Account] WHERE Id = @Id";
                parameters.Add("@Id", idAccount, DbType.Decimal);

                balance = await _unitOfWork.Connection.QuerySingleOrDefaultAsync<decimal>(select, parameters, _unitOfWork.Transaction);

                if (balance > 0)
                {
                    balance = decimal.Add(balance, value);
                    var update = @"UPDATE [Account] SET Balance = @Balance WHERE Id = @Id";
                    parameters.Add("@Balance", balance);

                    await _unitOfWork.Connection.ExecuteAsync(update, parameters, _unitOfWork.Transaction);
                }
                _unitOfWork.Commit();
                return balance;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
