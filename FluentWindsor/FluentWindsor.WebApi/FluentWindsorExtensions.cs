using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.WebApi
{
    public static class FluentWindsorExtensions
    {
        public static FluentlyWindsor.FluentWindsor RegisterApiControllers(this FluentlyWindsor.FluentWindsor fluentWindsor, HttpConfiguration configuration)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new FluentWindsorControllerActivator());
            //This method breaks webapi
            GlobalConfiguration.Configuration.DependencyResolver = new FluentWindsorDependencyResolver();
	        return fluentWindsor;
	        //return fluentWindsor.WithTypesInheriting<ApiController>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_API")).LifeStyle.PerWebRequest));
        }
    }
}