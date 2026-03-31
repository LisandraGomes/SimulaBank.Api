using Microsoft.Data.SqlClient;
using SimulaBank.Domain.Interfaces.Services;
using System.Data;

namespace SimulaBank.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        public IDbConnection Connection {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                     _connection.Open();
                return _connection;
            }
        }
        public IDbTransaction Transaction { get; private set; }


        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            Transaction = _connection.BeginTransaction();
        }
        public void Commit()
        {
            Transaction?.Commit();
            Dispose();
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            _connection?.Dispose();
        }

    }
}
