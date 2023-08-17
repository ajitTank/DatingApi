using DatingApi.Data;
using DatingApi.Model;
using DatingApi.Repository.IRepository;

namespace DatingApi.Repository
{
    public class AccountRepository : GenericRepository<AppUser>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public void saveChanges()
        {
           this.DbContext.SaveChanges();    
        }
    }
}
