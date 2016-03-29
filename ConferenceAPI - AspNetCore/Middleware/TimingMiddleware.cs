using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System.Diagnostics;

namespace ConferenceAPI.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class TimingMiddleware
    {
        private readonly RequestDelegate _next;

        public TimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            return _next(httpContext).ContinueWith((task) =>
            {
                stopWatch.Stop();
                httpContext.Response.Headers.Add("ExecutionTime", stopWatch.ElapsedMilliseconds.ToString());
            });
        }
    }


    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimingMiddleware>();
        }
    }
}
