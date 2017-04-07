using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentlyWindsor;
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

            FluentLifestyleLifetimeScope.GetCurrentLifetimeScope += () =>
            {
                return (FluentLifestyleLifetimeScope) HttpContext.Current.Items["fluentwindsor-lifetime-scope"];
            };

            FluentLifestyleLifetimeScope.DisposeLifetimeScope += () =>
            {
                return (FluentLifestyleLifetimeScope) HttpContext.Current.Items["fluentwindsor-lifetime-scope"];
            };

            FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .RegisterApiControllers(GlobalConfiguration.Configuration)
                .RegisterMvcControllers(ControllerBuilder.Current, "Example.Web.Controllers")
                .Create();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var scope = new FluentLifestyleLifetimeScope();
            HttpContext.Current.Items.Add("fluentwindsor-lifetime-scope", scope);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var scope = (FluentLifestyleLifetimeScope) HttpContext.Current.Items["fluentwindsor-lifetime-scope"];
            HttpContext.Current.Items.Remove("fluentwindsor-lifetime-scope");
            scope.Dispose();
            scope = null;
        }
    }
}