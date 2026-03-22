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
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
            var sql = @"SELECT u.Id,
                                FirstName AS Name, 
                                MidName, 
                                Cpf, 
                                Email, 
                                Password, 
                                BirthDate, 
                                RegistrationDate, 
                                EmailAthorization, 
                                IdRole,
                                r.RoleName AS RoleName
                         FROM [User] u
                         INNER JOIN [Role] r ON u.IdRole = r.Id
                         WHERE 
                                Email = @Email 
                                OR Cpf = @Cpf";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email, Cpf = cpf });
            }
        }

        public async Task<bool> CheckThePassword(string cpf, string passwordHash)
        {
            var sql = "SELECT Top 1 1 FROM [User] WHERE Password = @PasswordHash AND Cpf = @Cpf";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { PasswordHash = passwordHash, Cpf = cpf });
            }
        }

        public async Task InsertUser(string firstName, string midName, string cpf, string email, string passwordHash, DateTime birthDate, int idRole)
        {
            try
            {
                var sql = @"INSERT INTO [User] (FirstName, MidName, Cpf, Email, [Password], BirthDate, RegistrationDate,Active, EmailAthorization, IdRole)
                        VALUES (@FirstName, @MidName, @Cpf, @Email, @PasswordHash, @BirthDate, @RegistrationDate, 0, 0, @IdRole)";

                var user = new
                {
                    FirstName = firstName,
                    MidName = midName,
                    Cpf = cpf,
                    Email = email,
                    PasswordHash = passwordHash,
                    BirthDate = birthDate,
                    RegistrationDate = DateTime.Now,
                    IdRole = idRole
                };

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.ExecuteAsync(sql, user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the user: {ex.Message}");
                throw;
            }
        }
        public async Task UpdateAuthorizationEmail(Guid userId, bool emailAuthorization)
        {
            var sql = "UPDATE [User] SET EmailAthorization = @EmailAuthorization,DateEmailAuthorization = @Date WHERE Id = @UserId";

            var parameters = new
            {
                EmailAuthorization = emailAuthorization,
                Date = DateTime.Now,
                UserId = userId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

    }
}
