using System.Web.Mvc;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
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
        public static FluentWindsor RegisterMvcControllers(this FluentWindsor fluentWindsor, ControllerBuilder controllerBuilder, params string[] controllerNamespaces)
        {
            FluentWindsorExtensionsConstants.ControllerNamespaces = controllerNamespaces;
            ControllerBuilder.Current.SetControllerFactory(new FluentWindsorMvcControllerFactory(FluentWindsor.ServiceLocator));
            return fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name.Replace("Controller", "_MVC")).LifestyleCustom<PerWebRequestLifestyleManager>()));
        }
    }

    public class PerWebRequestLifestyleManager : AbstractLifestyleManager
    {
        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var service = base.Resolve(context, releasePolicy);

            return service;
        }

        public override void Dispose()
        {
            // TODO
        }
    }
}