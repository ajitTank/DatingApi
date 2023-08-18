using DatingApi.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApi.Utility
{
    public class JwtToken : IJwtToken
    {
        private readonly SymmetricSecurityKey key;
        public JwtToken(IConfiguration config)
        {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));   
        }
        public string Token(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name , user.UserName)
            };

         var Creds =   new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            
         var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
             Subject = new ClaimsIdentity(claims),
             Expires = DateTime.Now.AddSeconds(30),
             SigningCredentials = Creds
            };

         var jwtSecurityTokenHandler =   new JwtSecurityTokenHandler();
         var token =  jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
          return  jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
