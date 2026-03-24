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
        public async Task<List<Piggy>> GetAllByUserId(Guid id, bool onlyActive)
        {
            var sql = @"SELECT Id, [Title], [Description], GoalValue, CurrenteValue, [Status], CreateDate, DueDate, DayAutoDeductValueAccount, ActiveAutoDeduct, ValueAutoDeductValueAccount, Active, UserId
                        FROM [Piggy] WHERE UserId = @Id ";
            
            if(onlyActive)
            {
                sql += " AND Active = @Active";
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Piggy>(sql, new { Id = id, Active = onlyActive });
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
