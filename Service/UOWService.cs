using DatingApi.Data;
using DatingApi.Repository;
using DatingApi.Repository.IRepository;
using DatingApi.Service.IService;

namespace DatingApi.Service
{
    public class UOWService : IUOWService
    {
        public IAccountRepository accountRepository { get;private set; }
        public UOWService(ApplicationDbContext dbContext)
        {
            accountRepository = new AccountRepository(dbContext);
        }
    }
}
