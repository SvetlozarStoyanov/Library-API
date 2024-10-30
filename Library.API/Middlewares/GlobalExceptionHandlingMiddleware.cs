using Azure;
using Library.Core.Common.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace Library.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException e)
            {
                // TODO Log Exceptions in logger
                logger.LogError(e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Item not found!",
                    Detail = e.Message
                };
                string json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Invalid operation!",
                    Detail = e.Message
                };
                string json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (ArgumentException e)
            {
                logger.LogError(e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Invalid argument!",
                    Detail = e.Message
                };
                string json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Detail = "An internal server error has occured!"
                };

                context.Response.ContentType = "application/json";
                string json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
