using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNetCore.Mvc;
using FluentlyWindsor;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Mvc;
using Microsoft.AspNetCore.Builder;
using Castle.MicroKernel.Lifestyle;
using Microsoft.Extensions.DependencyInjection;

namespace FluentlyWindsor.AspNetCore
{
    public static class FluentWindsorExtensions
    {
        public static FluentlyWindsor.FluentWindsor RegisterAspNetCoreControllers(this FluentWindsor fluentWindsor, IServiceCollection services, Action<IWindsorContainer> configureContainer = null)
        {
			services.AddRequestScopingMiddleware(FluentWindsor.ServiceLocator.BeginScope);
	        services.AddCustomControllerActivation(FluentWindsor.ServiceLocator.Resolve);
			fluentWindsor.WithTypesInheriting<Controller>((x, y) => x.RegisterIfNotAlready(Component.For(y).Named(y.Name+ "_ASPNETCORE_C").LifeStyle.Scoped()));
	        configureContainer?.Invoke(FluentWindsor.ServiceLocator);
	        return fluentWindsor;
        }
    }
}