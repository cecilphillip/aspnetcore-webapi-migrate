using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ConferenceAPI.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class PingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string PingMe = "X-PingMe";
        private const string PingBack = "X-PingBack";
        private readonly ILogger _logger;

        public PingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._logger = loggerFactory.CreateLogger<PingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey(PingMe))
            {
                var value = headers[PingMe];
                _logger.LogDebug($"Pinging {value}");

                context.Response.Headers[PingBack] = $"Hi {value}";
                context.Response.StatusCode = (int)HttpStatusCode.Accepted;
                return;
            }

            await _next(context);
        }
    }

    public static class PingMiddlewareExtensions
    {
        public static IApplicationBuilder UsePing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PingMiddleware>();
        }
    }
}
