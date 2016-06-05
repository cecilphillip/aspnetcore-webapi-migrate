using System;
using System.IO;
using ConferenceAPI.Filters;
using ConferenceAPI.Middleware;
using ConferenceAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.SwaggerGen.Generator;

namespace ConferenceAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
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
            }).AddWebApiConventions();

            services.AddSwaggerGen(opts =>
            {
                opts.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Conference API"
                });
                opts.DescribeAllEnumsAsStrings();
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

            // Custom Middleware
            app.UseCustomHeader();
            app.UsePing();

            app.UseCors("default_policy");
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapWebApiRoute(
                    name: "apiRoute",
                    template: "api/{controller}/{action}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}