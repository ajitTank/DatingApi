using DatingApi.Data;
using DatingApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingApi.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbSet<T> dbEntitySet = null;
        public GenericRepository(ApplicationDbContext dbContext)
        {
           this.dbEntitySet = dbContext.Set<T>();
        }


        public async  void AddEntityAsync(T entity)
        {
            this.dbEntitySet.AddAsync(entity);   

        }

        public async void DeleteEntityAsync(T entity)
        {
            this.dbEntitySet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperty = null)
        {
            IQueryable<T> query = dbEntitySet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(includeProperty != null)
            {
                foreach (var item in includeProperty.Split(','))
                {
                    query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByValueAsync(Expression<Func<T, bool>> filter, string? includeProperty = null)
        {
            IQueryable<T> query = dbEntitySet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperty != null)
            {
                foreach (var item in includeProperty.Split(','))
                {
                    query.Include(item);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
    
}
