using System.Web.Http;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.WebApi
{
    public static class FluentWindsorExtensions
    {
        public static FluentlyWindsor.FluentWindsor RegisterApiControllers(this FluentlyWindsor.FluentWindsor fluentWindsor, HttpConfiguration configuration)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new FluentWindsorDependencyResolver();
            return fluentWindsor.WithTypesInheriting<ApiController>(
                (x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_API")).LifeStyle.PerWebRequest));
        }
    }
}