using System.Net;

namespace DatingApi.Utility
{
    public class ApiResponse
    {
        public bool? isSuccess { get; set; } = true;
        public HttpStatusCode httpStatusCode { get; set; }
        public List<string> ErrorMessage { get; set; }
        public Object Result { get; set; }
        public string Token { get; set; }

        public ApiResponse()
        {
            
        }
        public ApiResponse(bool isSuccess,HttpStatusCode httpStatusCode , List<string> ErrorMessage)
        {
            this.isSuccess = isSuccess;
            this.httpStatusCode = httpStatusCode;
            this.ErrorMessage = ErrorMessage;
        }

    }
}
