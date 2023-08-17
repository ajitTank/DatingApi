using DatingApi.Repository.IRepository;

namespace DatingApi.Service.IService
{
    public interface IUOWService
    {
        public IAccountRepository  accountRepository { get; }
    }

}
