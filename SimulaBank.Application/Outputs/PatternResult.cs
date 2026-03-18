using NPOI.SS.Formula.Functions;
using System.Net;

namespace SimulaBank.Application.Outputs
{
    public class PatternResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Object { get; set; }
        public Error? error { get; set; }

        public PatternResult(T item)
        {
            StatusCode = HttpStatusCode.OK;
            Object = item;
        }
        public PatternResult(HttpStatusCode statusCode,string message)
        {
            StatusCode = statusCode;
            Object = default(T);
            error = new Error(message);
        }
    }
    public class Error
    {
        public string Message { get; set; }
        internal Error(string message)
        {
            Message = message;
        }
    }

}
