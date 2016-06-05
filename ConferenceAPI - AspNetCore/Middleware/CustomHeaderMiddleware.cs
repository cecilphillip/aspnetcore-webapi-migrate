using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("Conference-Header", "This is my custom header.");
                return Task.CompletedTask;
            });
            
            await _next(context);            
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
