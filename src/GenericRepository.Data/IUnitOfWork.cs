using GenericRepository.Data.Entity;

namespace GenericRepository.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
    }
}
