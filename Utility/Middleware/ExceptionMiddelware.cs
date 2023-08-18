using System.Net;
using System.Text.Json;

namespace DatingApi.Utility.Middleware
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddelware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddelware(RequestDelegate next ,ILogger<ExceptionMiddelware> logger , IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //process the request further middleware
              await  next(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                /* context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;*/

                var response = env.IsDevelopment() ? new ApiResponse(false,HttpStatusCode.InternalServerError, new List<string>() { ex.Message, ex.StackTrace.ToString() })
                        : new ApiResponse(false,HttpStatusCode.InternalServerError, new List<string>() { ex.Message, "Internal Server Error" });
                
                var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, option);

                context.Response.WriteAsync(json);

            }
        }
    }
}
