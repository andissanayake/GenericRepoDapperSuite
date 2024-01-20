using Dapper.Contrib.Extensions;
using GenericRepository.Data.Entity;
using System.Data;

namespace GenericRepository.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;


        public GenericRepository(IDbTransaction dbTransaction)
        {
            _dbConnection = dbTransaction.Connection ?? default!;
            _dbTransaction = dbTransaction;
            SqlMapperExtensions.TableNameMapper = (type) => type.Name;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbConnection.GetAllAsync<TEntity>(_dbTransaction);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbConnection.GetAsync<TEntity>(id, _dbTransaction);
        }

        public async Task InsertAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = null;
            await _dbConnection.InsertAsync(entity, _dbTransaction);
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            entityToUpdate.UpdatedDate = DateTime.UtcNow;
            await _dbConnection.UpdateAsync<TEntity>(entityToUpdate, _dbTransaction);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            await _dbConnection.DeleteAsync(entity, _dbTransaction);
        }
    }

}
