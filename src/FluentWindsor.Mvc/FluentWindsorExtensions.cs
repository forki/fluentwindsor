using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Lifestyle;

namespace FluentlyWindsor.Mvc
{
    public static class FluentWindsorExtensions
    {
        public static FluentWindsor RegisterMvcControllers(this FluentWindsor fluentWindsor, ControllerBuilder controllerBuilder, params string[] controllerNamespaces)
        {
            FluentWindsorExtensionsConstants.ControllerNamespaces = controllerNamespaces;
            ControllerBuilder.Current.SetControllerFactory(new FluentWindsorMvcControllerFactory(FluentWindsor.ServiceLocator));
            return fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_MVC")).LifestyleCustom<FluentLifestyleManager>()));
        }
    }
}