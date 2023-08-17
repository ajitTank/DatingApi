using DatingApi.Model;
using DatingApi.Model.DTO;
using DatingApi.Repository.IRepository;
using DatingApi.Service.IService;
using DatingApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUOWService UOW;
        protected ApiResponse response;
        private readonly IJwtToken jwtToken;

        public AccountController(IUOWService UOW , IJwtToken jwtToken)
        {
            this.UOW = UOW;
            this.jwtToken = jwtToken;
            this.response = new();
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest , Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
// its not capturing the interner server error
// how to to exception handling ?

        public async Task<ActionResult<ApiResponse>> register(RegisterDto registerDto)
        {
           
            try
            {
                //here the saving data depends on the user existence and userExit is async type
                // how to handel the 
                var UserExist = await userExistAsync(registerDto.UserName);
                if ( UserExist!= null)
                {
                    this.response.httpStatusCode = HttpStatusCode.BadRequest;
                    this.response.ErrorMessage = new List<string>() { "User Name Already exist" };
                    return this.response;
                }
                

                using var hmc = new HMACSHA512();

                var appUser = new AppUser()
                {
                    UserName = registerDto.UserName,
                    PasswordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmc.Key
                };
                this.UOW.accountRepository.AddEntityAsync(appUser);
                this.UOW.accountRepository.saveChanges();

                SuccessResponse(HttpStatusCode.OK,appUser);
                return this.response;
            }
            catch (Exception ex)
            {
                ErrorResponse(new List<string>() { ex.ToString() }, HttpStatusCode.InternalServerError);
                return this.response;
               
            }
                
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.response.httpStatusCode = HttpStatusCode.BadRequest;
                    this.response.ErrorMessage = new List<string>() { ModelState.ToString() };
                    return this.response;
                }
                var userExist = await userExistAsync(loginDto.userName);
                if ( userExist == null)
                {
                    {
                        ErrorResponse(new List<string>() { "User Name not found" }, HttpStatusCode.NotFound);
                        return this.response;
                    }
                }
                //here passing the hmc.key to hte HMACSHA512 the ctor generate the same hasCode
                using var hmc = new HMACSHA512(userExist.PasswordSalt);
                 var hmcCode =  hmc.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                

               for(int i = 0; i<hmcCode.Length; i++)
                {
                    if (hmcCode[i] != userExist.PasswordHash[i])
                    {

                        ErrorResponse(new List<string>() { "Password Name not found" }, HttpStatusCode.NotFound);
                        return this.response;
                    }
                        
                }

                var token = jwtToken.Token(userExist);
               SuccessResponse(HttpStatusCode.OK,userExist,token);
                return this.response;   
            }
            catch (Exception ex)
            {

                ErrorResponse(new List<string>() { ex.ToString() }, HttpStatusCode.InternalServerError);
                return this.response;
            }
        }


        [NonAction]
        public void SuccessResponse(HttpStatusCode httpStatusCode,Object result , string token )
        {
            this.response.httpStatusCode = httpStatusCode;
            this.response.Result = result;
            this.response.Token = token;
        }
        [NonAction]
        public void SuccessResponse(HttpStatusCode httpStatusCode, Object result)
        {
            this.response.httpStatusCode = httpStatusCode;
            this.response.Result = result;
        }

        [NonAction]
        public void ErrorResponse(List<string> message , HttpStatusCode httpStatusCode)
        {
            this.response.isSuccess = false;
            this.response.ErrorMessage = message;
            this.response.httpStatusCode = httpStatusCode;
        }
        [NonAction]
        public async Task<AppUser> userExistAsync(string userName)
        {
           var userExist = await this.UOW.accountRepository.GetByValueAsync(x => x.UserName.ToLower() == userName.ToLower());
            if (userExist == null)
            {
                return null;
            }
            return userExist;
              
        }
    }
}
