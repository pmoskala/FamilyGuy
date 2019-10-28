using FamilyGuy.Contracts.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FamilyGuy.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const string errorCode = "error";
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            Type exceptionType = exception.GetType();
            statusCode = exception switch
            {
                { } when exceptionType == typeof(UnauthorizedAccessException) => HttpStatusCode.Unauthorized,
                { } when exceptionType.BaseType == typeof(FgBaseNotFoundException) => HttpStatusCode.NotFound,
                { } when exceptionType == typeof(Exception) => HttpStatusCode.InternalServerError,
                _ => statusCode
            };

            CustomExceptionResponse response = new CustomExceptionResponse
            {
                Code = errorCode,
                Message = exception.Message
            };
            string payload = JsonConvert.SerializeObject(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(payload);
        }
    }

    internal class CustomExceptionResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
