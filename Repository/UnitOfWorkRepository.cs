using DatingApi.Data;
using DatingApi.Repository.IRepository;

namespace DatingApi.Repository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        public IAccountRepository accountRepository { get;private set; }

        public UnitOfWorkRepository(ApplicationDbContext dbContext)
        {
           accountRepository = new AccountRepository(dbContext);
        }
    }
}
