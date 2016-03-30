using System;
using ConferenceAPI.Filters;
using ConferenceAPI.Middleware;
using ConferenceAPI.Models;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.Http.Features;
using Swashbuckle.SwaggerGen;

namespace ConferenceAPI
{
    public class Startup
    {
        public Startup()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default_policy", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins("https://my-cool-site.com");
                });
            });
            services.AddSingleton<ValidateModelAttribute>();
            services.AddSingleton<IDataStore, DataStore>();

            services.AddMvc().AddJsonOptions(options =>
            {              
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(options =>
            {

                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Conference API"
                });
            });

            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async ctx =>
                    {
                     
                        var errorFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                        var error = errorFeature.Error; // Do what you want with this error

                        var responseData = new
                        {
                            Message = "Sorry, something when wrong. Please try again later",
                            DateTime = DateTimeOffset.Now
                        };

                        await ctx.Response.WriteAsync(JsonConvert.SerializeObject(responseData));
                    });
                });
            }

            app.Run(context =>
            {
                throw new Exception("Fire in the whole!!");
            });

            app.UseIISPlatformHandler();
            app.UseCustomHeader();
            app.UsePing();

            app.UseCors("default_policy");
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "apiRoute",
                    template: "api/{controller}/{action}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
