using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentlyWindsor;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;

namespace Example.MVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .RegisterApiControllers(GlobalConfiguration.Configuration)
                .RegisterMvcControllers(ControllerBuilder.Current, "Example.MVC.Controllers", "Another.Namespace.For.Controllers")
                .Create();
        }
    }
}