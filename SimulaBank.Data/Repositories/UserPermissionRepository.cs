using Dapper;
using Microsoft.Data.SqlClient;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;

namespace SimulaBank.Data.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly string _connectionString;
        public UserPermissionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Permission>> GetPermissions(Guid userId)
        {
            var sql = @"SELECT p.Id, p.[Description], p.Active FROM UserPermissions up INNER JOIN Permission p ON up.PermissionId = p.Id WHERE up.UserId = @UserId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.QueryAsync<Permission>(sql, new { UserId = userId });
                return result.Result.ToList();
            }

        }
    }
}
