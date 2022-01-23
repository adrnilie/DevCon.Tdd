using System.Net;

namespace DevCon.Code.Common
{
    public class Response
    {
        protected Response() { }

        public int StatusCode { get; set; }
        public bool Success => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; set; }

        public static Response ForSuccess()
        {
            return new Response
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        public static Response WithError(HttpStatusCode statusCode, string errorMessage)
        {
            return new Response
            {
                StatusCode = (int)statusCode,
                ErrorMessage = errorMessage
            };
        }
    }

    public class Response<TResult> : Response
    {
        public TResult Result { get; set; }

        public static Response<TResult> WithResult(TResult result)
        {
            return new Response<TResult>
            {
                Result = result,
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        public new static Response<TResult> WithError(HttpStatusCode statusCode, string errorMessage)
        {
            return new Response<TResult>
            {
                StatusCode = (int)statusCode,
                ErrorMessage = errorMessage
            };
        }
    }
}