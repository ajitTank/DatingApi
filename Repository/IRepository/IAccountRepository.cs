using DatingApi.Model;

namespace DatingApi.Repository.IRepository
{
    public interface IAccountRepository:IGenericRepository<AppUser>
    {
        public void saveChanges();
    }

}
