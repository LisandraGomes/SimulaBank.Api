using Dapper;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using System.Data;

namespace SimulaBank.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var sql = @"SELECT u.Id AS Id,
                                a.Id AS IdAccount,
                                u.Email AS Email,
                                u.FirstName AS Name
                        FROM [User] u 
                             LEFT JOIN [Account] a ON a.UserId = u.Id 
                        WHERE u.Id = @Id";

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id }, transaction: _unitOfWork.Transaction);
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
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email, DbType.String);
            parameters.Add("@Cpf", cpf, DbType.String);
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<User>(sql, parameters, transaction: _unitOfWork.Transaction);
        }

        public async Task<bool> CheckThePassword(string cpf, string passwordHash)
        {
            var sql = "SELECT Top 1 1 FROM [User] WHERE Password = @PasswordHash AND Cpf = @Cpf";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<bool>(sql, new { PasswordHash = passwordHash, Cpf = cpf }, transaction: _unitOfWork.Transaction);
        }

        public async Task<Guid> InsertUser(string firstName, string midName, string cpf, string email, string passwordHash, DateTime birthDate, int idRole)
        {
            try
            {
                var sql = @"INSERT INTO [User] (FirstName, MidName, Cpf, Email, [Password], BirthDate, RegistrationDate,Active, EmailAthorization, IdRole)
                            OUTPUT INSERTED.Id                          
                            VALUES (@FirstName, @MidName, @Cpf, @Email, @PasswordHash, @BirthDate, @RegistrationDate, 0, 0, @IdRole)";


                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", firstName, DbType.String);
                parameters.Add("@MidName", midName, DbType.String);
                parameters.Add("@Cpf", cpf, DbType.String);
                parameters.Add("@Email", email, DbType.String);
                parameters.Add("@PasswordHash", passwordHash, DbType.String);
                parameters.Add("@BirthDate", birthDate, DbType.DateTime);
                parameters.Add("@RegistrationDate", DateTime.Now, DbType.DateTime);
                parameters.Add("@IdRole", idRole, DbType.Int32);

                var result = await _unitOfWork.Connection.QuerySingleAsync<Guid>(sql, parameters, transaction: _unitOfWork.Transaction);
                //_unitOfWork.Commit();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the user: {ex.Message}");
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task UpdateAuthorizationEmail(Guid userId, bool emailAuthorization)
        {
            try
            {
                var sql = "UPDATE [User] SET EmailAthorization = @EmailAuthorization,DateEmailAuthorization = @Date WHERE Id = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@EmailAuthorization", emailAuthorization, DbType.String);
                parameters.Add("@Date", DateTime.Now, DbType.DateTime);
                parameters.Add("@UserId", userId, DbType.Guid);

                await _unitOfWork.Connection.ExecuteAsync(sql, parameters, transaction: _unitOfWork.Transaction);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while update authorization the user: {ex.Message}");
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> ActiveUser(Guid userId, string email)
        {
            try
            {
                var sql = @"UPDATE [User] SET Active = @Active WHERE Id = @UserId AND Email = @Email";
                var parameters = new DynamicParameters();
                parameters.Add("@Active", true, DbType.Boolean);
                parameters.Add("@UserId", userId, DbType.Guid);
                parameters.Add("@Email", email, DbType.String);

                var affectedRows = await _unitOfWork.Connection.ExecuteAsync(sql, parameters, transaction: _unitOfWork.Transaction);
                _unitOfWork.Commit();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while update active the user: {ex.Message}");
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
