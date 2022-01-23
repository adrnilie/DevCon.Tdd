using System.Net;

namespace DevCon.Code.Common
{
    public class ServiceResponse
    {
        protected ServiceResponse(){ }

        public int StatusCode { get; set; }
        public bool Success => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; set; }

        public static ServiceResponse ForSuccess()
        {
            return new ServiceResponse
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        public static ServiceResponse WithError(HttpStatusCode statusCode, string errorMessage)
        {
            return new ServiceResponse
            {
                StatusCode = (int) statusCode,
                ErrorMessage = errorMessage
            };
        }
    }

    public class ServiceResponse<TResult> : ServiceResponse
    {
        public TResult Result { get; set; }

        public static ServiceResponse<TResult> WithResult(TResult result)
        {
            return new ServiceResponse<TResult>
            {
                Result = result,
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        public new static ServiceResponse<TResult> WithError(HttpStatusCode statusCode, string errorMessage)
        {
            return new ServiceResponse<TResult>
            {
                StatusCode = (int)statusCode,
                ErrorMessage = errorMessage
            };
        }
    }
}