using System.Data;

namespace SimulaBank.Domain.Interfaces.Services
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();

    }
}
