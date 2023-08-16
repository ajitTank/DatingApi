using System.Linq.Expressions;

namespace DatingApi.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>?filter = null ,string? includeProperty = null);

        public Task<T> GetByValueAsync(Expression<Func<T, bool>> filter, string? includeProperty = null);

        public void AddEntityAsync(T entity);

        public void DeleteEntityAsync(T entity);

    }
}
