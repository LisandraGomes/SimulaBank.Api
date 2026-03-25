using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;
using System.Data;

namespace SimulaBank.Data.Repositories
{
    public class PiggyRepository : IPiggyRepository
    {
        private readonly string _connectionString;
        public PiggyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Piggy>> GetAllByUserId(Guid id, bool onlyActive)
        {
            var sql = @"SELECT Id, [Title], [Description], GoalValue, CurrenteValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId
                        FROM [Piggy] WHERE UserId = @Id ";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Guid);
            if(onlyActive)
            {
                sql += " AND Active = @Active";
                parameters.Add("@Active", onlyActive, DbType.Boolean);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Piggy>(sql, parameters);
                return result.ToList();
            }
        }
        public async Task<Piggy> GetById(Guid id)
        {
            var sql = @"SELECT Id, [Title], [Description], GoalValue, CurrenteValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId FROM [Piggy] WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstOrDefaultAsync<Piggy>(sql, new { Id = id });
                return result;
            }
        }
        public async Task<Guid> Create(string title, string description, decimal currentValue, int status, DateTime createDate, DateTime dueDate, int dayAutoDeduct, decimal valueAutoDeduct, bool ActiveAutoDeduct, bool active, Guid userId)
        {

            var sql = @"INSERT INTO
                        [Piggy] ([Title], [Description], GoalValue, CurrenteValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId)
                        OUTPUT INSERTED.Id                        
                        VALUES (@Title, @Description, @GoalValue, @CurrentValue, @Status, @CreateDate, @DueDate, @DayAutoDeductValueAccount, @ActiveAutoDeduct, @ValueAutoDeductValueAccount, @Active, @UserId)";

            var parameters = new DynamicParameters();
            parameters.Add("@Title", title, DbType.String);
            parameters.Add("@Description", description, DbType.String);
            parameters.Add("@GoalValue", currentValue, DbType.Decimal);
            parameters.Add("@CurrentValue", currentValue, DbType.Decimal);
            parameters.Add("@Status", status, DbType.Int32);
            parameters.Add("@CreateDate", createDate, DbType.DateTime);
            parameters.Add("@DueDate", dueDate, DbType.DateTime);
            parameters.Add("@DayAutoDeductValueAccount", dayAutoDeduct, DbType.Int32);
            parameters.Add("@ActiveAutoDeduct", ActiveAutoDeduct, DbType.Boolean);
            parameters.Add("@ValueAutoDeductValueAccount", valueAutoDeduct, DbType.Decimal);
            parameters.Add("@Active", active, DbType.Boolean);
            parameters.Add("@UserId", userId, DbType.Guid);

            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<Guid>(sql, parameters);
                return id;
            }
        }

        public async Task Inative(Guid id)
        {
            var sql = @"UPDATE [Piggy] SET Active = 0 WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }




    }
}
