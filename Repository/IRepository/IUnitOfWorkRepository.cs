namespace DatingApi.Repository.IRepository
{
    public interface IUnitOfWorkRepository
    {
        public IAccountRepository accountRepository { get;  }
    }
}
