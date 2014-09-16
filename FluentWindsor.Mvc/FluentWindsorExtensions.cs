using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.Mvc
{
    public static class FluentWindsorExtensionsConstants
    {
        public static string[] ControllerNamespaces = new string[0];
    }

    public static class FluentWindsorExtensions
    {
        public static FluentlyWindsor.FluentWindsor RegisterMvcControllers(this FluentlyWindsor.FluentWindsor fluentWindsor, System.Web.Mvc.ControllerBuilder controllerBuilder, params string[] controllerNamespaces)
        {
            FluentWindsorExtensionsConstants.ControllerNamespaces = controllerNamespaces;
            ControllerBuilder.Current.SetControllerFactory(new FluentWindsorMvcControllerFactory(FluentlyWindsor.FluentWindsor.ServiceLocator));
            return fluentWindsor.WithTypesInheriting<Controller>(
                (x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_MVC")).LifeStyle.PerWebRequest));
        }
    }
}