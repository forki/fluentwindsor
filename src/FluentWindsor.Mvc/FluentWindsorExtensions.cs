using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.Mvc
{
    public static class FluentWindsorExtensions
    {
        public static FluentWindsor RegisterMvcControllers(this FluentWindsor fluentWindsor, ControllerBuilder controllerBuilder, params string[] controllerNamespaces)
        {
            FluentWindsorExtensionsConstants.ControllerNamespaces = controllerNamespaces;
            ControllerBuilder.Current.SetControllerFactory(new FluentWindsorMvcControllerFactory(FluentWindsor.ServiceLocator));

            // Conundrum, externally tracked burdens only release root instance and not entire dependency graph, internally tracked budrens release children but hold on to root where finalisers are not called using this method
            //return fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_MVC")).LifestyleCustom<PerWebRequestLifestyleManager>()));

            return fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_MVC")).LifestyleTransient()));
        }
    }
}