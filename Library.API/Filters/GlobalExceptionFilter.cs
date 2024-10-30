using Library.Core.Common.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Library.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            Type exceptionType = exception.GetType();

            if (exception.GetType() == typeof(NotFoundException))
            {
                string response = exception.Message;

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            else if (exception.GetType() == typeof(InvalidOperationException)
                || exception.GetType() == typeof(ArgumentException))
            {
                string response = exception.Message;

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            else
            {
                object response = new
                {
                    //message = exception.Message,
                    message = "Server error!"
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
