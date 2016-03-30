using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using ConferenceAPI;
using ConferenceAPI.Models;
using Microsoft.Owin;
using Owin;
using WebActivatorEx;
using ConferenceAPI.Filters;
using Swashbuckle.Application;
using ConferenceAPI.Handlers;

[assembly: System.Web.PreApplicationStartMethod(typeof(Startup), "PreStart")]
[assembly: PostApplicationStartMethod(typeof(Startup), "PostStart")]
[assembly: ApplicationShutdownMethod(typeof(Startup), "Stop")]
[assembly: OwinStartup(typeof(Startup))]

namespace ConferenceAPI
{
    public class Startup
    {
        private static IContainer _container;
        public static void PreStart()
        {
            _container = BuildContainer();

            //only supporting the Razor engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DataStore>().As<IDataStore>().SingleInstance();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            return builder.Build();
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(_container);
            ConfigureWebAPI(app);
        }

        private void ConfigureWebAPI(IAppBuilder app)
        {
            // Web API routes and configuration
            var config = new HttpConfiguration();

            config.EnableCors(new CorsDemoAttribute()); // Registers CoreMessageHandler

            config.EnableSwagger("docs/{apiVersion}", c => c.SingleApiVersion("v1", "Conference API"))
                .EnableSwaggerUi();

            config.MessageHandlers.Add(new CustomHeaderHandler());
            config.MessageHandlers.Insert(0, new PingHandler());

            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "apiRoute",
                routeTemplate: "api/{controller}/{action}");

            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);


            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        public static void PostStart()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
