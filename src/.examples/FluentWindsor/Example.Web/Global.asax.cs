using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Lifestyle.Scoped;
using FluentlyWindsor;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Lifestyle;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;

namespace Example.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .WithFluentLifetimeScope(() => (ILifetimeScope) HttpContext.Current.Items["fluentwindsor-lifetime-scope"])
                .RegisterApiControllers(GlobalConfiguration.Configuration)
                .RegisterMvcControllers(ControllerBuilder.Current, "Example.Web.Controllers")
                .Create();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var scope = new FluentLifetimeScope();

            HttpContext.Current.Items.Add("fluentwindsor-lifetime-scope", scope);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var scope = (FluentLifetimeScope) HttpContext.Current.Items["fluentwindsor-lifetime-scope"];

            HttpContext.Current.Items.Remove("fluentwindsor-lifetime-scope");

            scope.Dispose();
        }
    }
}