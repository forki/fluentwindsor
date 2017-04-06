using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Lifestyle.Scoped;
using FluentlyWindsor;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;

namespace Example.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //this.BeginRequest += (s, e) =>
            //{
            //    var lifeTimeScope = new DefaultLifetimeScope(new ScopeCache());
            //    System.Web.HttpContext.Current.Session["fluentwindsor-lifetime-scope"] = lifeTimeScope;
            //};

            //this.EndRequest += (s, e) =>
            //{
            //    var lifeTimeScope = (DefaultLifetimeScope) System.Web.HttpContext.Current.Session["fluentwindsor-lifetime-scope"];
            //    lifeTimeScope.Dispose();
            //};

            FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .RegisterApiControllers(GlobalConfiguration.Configuration)
                .RegisterMvcControllers(ControllerBuilder.Current, "Example.Web.Controllers")
                .Create();
        }
    }
}
