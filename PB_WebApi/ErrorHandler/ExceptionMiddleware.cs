using Domain.Exceptions;
using PresentationModels.Models;
using System.Net;

namespace PB_WebApi.ErrorHandler
{
    public class ExceptionMiddleware : IMiddleware
    {
        private const string SOME_SERVER_ERROR = "Some internal server error was happened";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (ex is LowPrivilegesLevelException)
            {
                context.Response.Headers.Add("Low-Privileges-Level", "true");
            }

            if (ex is InvalidTokenException
                || ex is InvalidRefreshTokenException)
            {
                context.Response.Headers.Add("Invalid-Token", "true");
            }

            if (ex is EmptyCollectionException
                    || ex is InvalidLoginDataException
                    || ex is InvalidTokenException
                    || ex is ItemAlreadyExistsException
                    || ex is ItemNotExistsException
                    || ex is LowPrivilegesLevelException
                    || ex is NullValueException
                    || ex is TokenExpiredException
                    || ex is WrongCollectionItemsCountException
                    || ex is WrongOperationException
                    || ex is InvalidRefreshTokenException
                    || ex is ArgumentException)
            {
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = ex.Message
                }
                .ToString());
            }
            else
            {
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = SOME_SERVER_ERROR
                }
                .ToString());
            }
        }
    }
}
