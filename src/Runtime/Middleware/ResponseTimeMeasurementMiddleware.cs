using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FamilyGuy.Middleware
{
    public class ResponseTimeMeasurementMiddleware
    {
        private const string ResponseHeaderResponseTime = "X-Response-Time-ms";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ResponseTimeMeasurementMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("Performance measurement");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            httpContext.Response.OnStarting(() =>
            {
                watch.Stop();
                long responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                httpContext.Response.Headers[ResponseHeaderResponseTime] = responseTimeForCompleteRequest.ToString();
                _logger.LogWarning($"Response completed in {responseTimeForCompleteRequest} ms.");

                return Task.CompletedTask;
            });
            await _next(httpContext);
        }
    }

    public static class ResponseTimeMeasurementMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTimeMeasurementMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMeasurementMiddleware>();
        }
    }
}
