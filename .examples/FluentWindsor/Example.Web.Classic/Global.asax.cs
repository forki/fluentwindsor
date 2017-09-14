using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Example.Web.Classic.Controllers;
using FluentlyWindsor;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;

namespace Example.Web.Classic
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

	        var container = FluentWindsor
		        .NewContainer(Assembly.GetExecutingAssembly())
		        .WithArrayResolver()
		        .WithInstallers()
		        .RegisterMvcControllers(ControllerBuilder.Current)
		        .RegisterApiControllers(GlobalConfiguration.Configuration)
		        .Create();

	        container.Register(Component.For<IAnyService>().ImplementedBy<AnyService>());
        }
    }
}
