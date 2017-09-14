using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.Mvc
{
    public static class FluentWindsorExtensions
    {
        public static FluentlyWindsor.FluentWindsor RegisterMvcControllers(this FluentlyWindsor.FluentWindsor fluentWindsor, System.Web.Mvc.ControllerBuilder controllerBuilder)
        {
            ControllerBuilder.Current.SetControllerFactory(new FluentWindsorMvcControllerFactory(FluentlyWindsor.FluentWindsor.ServiceLocator));
            return fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name+ "_MVC").LifeStyle.PerWebRequest));
        }
    }
}