using GenericRepository.Data.Entity;
using System.Collections.Concurrent;
using System.Data;

namespace GenericRepository.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction { get; set; }

        private readonly ConcurrentDictionary<Type, object> _repositories;
        private bool _disposed = false;
        public UnitOfWork(
            IDbConnection dbConnection)
        {
            _repositories = new ConcurrentDictionary<Type, object>();
            _dbConnection = dbConnection;
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();

        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            return _repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(_dbTransaction)) as IGenericRepository<T> ?? default!;
        }
        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch
            {
                _dbTransaction.Rollback();
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _repositories.Clear();
                _dbConnection?.Close();
                _dbConnection?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                _dbTransaction.Rollback();
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbTransaction.Dispose();
                _repositories.Clear();
                _dbConnection?.Close();
                _dbConnection?.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _dbTransaction?.Dispose();
                    _dbConnection?.Close();
                    _dbConnection?.Dispose();
                }

                // Dispose unmanaged resources.

                _disposed = true;
            }
        }
    }

}
