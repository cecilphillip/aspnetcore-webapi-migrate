using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using ConferenceAPI;
using Microsoft.Owin;
using Owin;
using WebActivatorEx;

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
