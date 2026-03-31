using Dapper;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using System.Data;

namespace SimulaBank.Data.Repositories
{
    public class HistoryEmailsRepository : IHistoryEmailsRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public HistoryEmailsRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(string emailUser, int patternEmailId, bool send, DateTime? dateSend)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EmailUser", emailUser, DbType.String);
            parameters.Add("@PatternEmailId", patternEmailId, DbType.Int32);
            parameters.Add("@Send", send, DbType.Boolean);
            parameters.Add("@DateCreate", DateTime.Now, DbType.DateTime2);
            parameters.Add("@DateSend", dateSend, DbType.DateTime2);

            var sql = @"INSERT INTO [HistoryEmails] 
                       (EmailUser, PatternEmailId, [Send], [DateCreate], [DateSend])
                        VALUES 
                       (@EmailUser, @PatternEmailId, @Send, @DateCreate, @DateSend);";

            await _unitOfWork.Connection.ExecuteAsync(sql, parameters, _unitOfWork.Transaction);
        }

        public async Task UpdateAcepted(int id, bool send, DateTime? dateSend)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Send", send, DbType.Boolean);
            parameters.Add("@DateSend", dateSend, DbType.DateTime2);
            parameters.Add("@Id", id, DbType.Int32);

            var sql = @"UPDATE [HistoryEmails]
                               [Send] = @Send,
                               DateSend = @DateSend
                         WHERE Id = @Id;";

            await _unitOfWork.Connection.ExecuteAsync(sql, parameters, _unitOfWork.Transaction);
        }
    }
}
