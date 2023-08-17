using DatingApi.Model;

namespace DatingApi.Utility
{
    public interface IJwtToken
    {
        public string Token(AppUser user);
    }

}
