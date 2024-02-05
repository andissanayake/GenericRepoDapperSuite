using System.Data.SqlClient;

namespace GenericRepository.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var connection = new SqlConnection(_connectionString);
            return new UnitOfWork(connection);
        }
    }
}
