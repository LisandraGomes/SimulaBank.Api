using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SimulaBank.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<User>> GetUserById(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<User>("SELECT * FROM Usuarios");
            }
        }

        public async Task<User> GetUserByLogin(string? email, string? cpf)
        {
            var sql = @"SELECT Id,
                                FirstName, 
                                MidName, 
                                Cpf, 
                                Email, 
                                Password, 
                                BirthDate, 
                                RegistrationDate, 
                                EmailAthorization, 
                                IdRole 
                         FROM [User] 
                         WHERE 
                                Email = @Email 
                                OR Cpf = @Cpf";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email, Cpf = cpf });
            }
        }

        public async Task<bool> CheckThePassword(string cpf, string passwordHash)
        {
            var sql = "SELECT Top 1 FROM [User] WHERE PasswordHash = @PasswordHash AND Cpf = @Cpf";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { PasswordHash = passwordHash, Cpf = cpf });
            }
        }
    }
}
