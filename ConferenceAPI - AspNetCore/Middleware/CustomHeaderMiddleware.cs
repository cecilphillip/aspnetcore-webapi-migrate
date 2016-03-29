using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace ConferenceAPI.Middleware
{
    // You may need to install the Microsoft.AspNet.Http.Abstractions package into your project
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //TODO:
            return _next(httpContext).ContinueWith((task)=> {
                httpContext.Response.Headers.Add("conference-header", "This is my custom header.");                
            });
        }
    }

 
    public static class CustomHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomHeaderMiddleware>();
        }
    }
}
