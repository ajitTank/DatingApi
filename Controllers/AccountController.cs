using DatingApi.Model;
using DatingApi.Model.DTO;
using DatingApi.Repository.IRepository;
using DatingApi.Service.IService;
using DatingApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IUOWService UOW { get; set; }
        protected ApiResponse response;
        public AccountController(IUOWService UOW)
        {
            this.UOW = UOW;
            this.response = new();
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> register(RegisterDto registerDto)
        {
           
            try
            {
                if (await this.UOW.accountRepository.GetByValueAsync(x => x.UserName.ToLower() == registerDto.UserName.ToLower()) != null)
                {
                    ErrorResponse(new List<string>() { "User Name is exist" }, HttpStatusCode.BadRequest);
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


        [NonAction]
        public void SuccessResponse(HttpStatusCode httpStatusCode,Object result)
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
    }
}
