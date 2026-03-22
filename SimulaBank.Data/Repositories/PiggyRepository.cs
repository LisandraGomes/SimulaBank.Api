using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Repositories;

namespace SimulaBank.Data.Repositories
{
    public class PiggyRepository : IPiggyRepository
    {
        private readonly string _connectionString;
        public PiggyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Piggy>> GetAllPiggyByUserId(Guid id)
        {
            var sql = @"SELECT Id, [Title], [Description], GoalValue, CurrentValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId FROM [Piggy] WHERE UserId = @Id ;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Piggy>(sql, new { Id = id });
                return result.ToList();
            }
        }
        public async Task<Piggy> GetPiggyById(Guid id)
        {
            var sql = @"SELECT Id, [Title], [Description], GoalValue, CurrentValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId FROM [Piggy] WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstOrDefaultAsync<Piggy>(sql, new { Id = id });
                return result;
            }
        }
        public async Task CreatePiggy(string title, string description, decimal currentValue, int status, DateTime createDate, DateTime dueDate, int dayAutoDeduct, decimal valueAutoDeduct, bool ActiveAutoDeduct, bool active, Guid userId)
        {
            var sql = @"INSERT INTO
                        [Piggy] ([Title], [Description], GoalValue, CurrentValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId)
                        VALUES (@Title, @Description, @GoalValue, @CurrentValue, @Status, @CreateDate, @DueDate, @DayAutoDeductValueAccount, @ActiveAutoDeduct, @ValueAutoDeductValueAccount, @Active, @UserId)";
            
            var parameters = new
            {
                Title = title,
                Description = description,
                GoalValue = currentValue,
                CurrentValue = currentValue,
                Status = status,
                CreateDate = createDate,
                DueDate = dueDate,
                DayAutoDeductValueAccount = dayAutoDeduct,
                ActiveAutoDeduct,
                ValueAutoDeductValueAccount = valueAutoDeduct,
                Active = active,
                UserId = userId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
