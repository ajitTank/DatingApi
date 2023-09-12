using DatingApi.Model;
using DatingApi.Model.DTO;
using DatingApi.Repository.IRepository;
using DatingApi.Service.IService;
using DatingApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly IUOWService unitOW;
        private readonly ApiResponse response ;

        public BuggyController(IUOWService unitOW)
        {
            this.unitOW = unitOW;
            response = new ApiResponse();
        }
        [HttpGet("server-error")]
        public async Task<ActionResult<ApiResponse>>serverError()
        {
            var value = await this.unitOW.accountRepository.GetByValueAsync(x => x.Id == -1);
            var valueToString = value.ToString();
            return Ok(valueToString);
           /* try
            {
                var value = await this.unitOW.accountRepository.GetByValueAsync(x => x.Id == -1);
                var valueToString = value.ToString();
                return Ok(valueToString);
            }
            catch (Exception ex)
            {
                this.response.ErrorMessage = new List<string>() { ex.ToString() };
                this.response.httpStatusCode = (System.Net.HttpStatusCode)StatusCodes.Status500InternalServerError;
                return response;
            }*/
            
        }

        [HttpGet("success")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(ApiResponse))]

        public ActionResult<ApiResponse> getSuccess()
        {
            this.response.Result = "I am success";
            return Ok("this.response") ;
        }

        [HttpGet("bad-request",Name ="400")]
     /*   [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(ApiResponse))]*/
        public ActionResult<ApiResponse> getBadRequest()
        {
            this.response.ErrorMessage = new List<string>() { "This is a bad request" };
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> getSecret()
        {
            return Ok("you are autorized");
        }

        [HttpGet("not-found",Name = "404")]
        public async Task<ActionResult<AppUser>> getNotFound()
        {
          var user = await  this.unitOW.accountRepository.GetByValueAsync(x=>x.Id == -1);
            if(user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("ValidationError")]
        public ActionResult<ApiResponse> postValidation([FromBody] LoginDto app)
        {
            if(!ModelState.IsValid)
            {
                this.response.ErrorMessage = new List<string>() { ModelState.ValidationState.ToString() };
                return BadRequest(this.response);
            }
            this.response.Result = "Validation Success";
            return Ok(this.response);
        }

    }
}
