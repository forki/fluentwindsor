using System;
using Castle.MicroKernel.Lifestyle.Scoped;
using FluentlyWindsor.Lifestyle;

namespace FluentlyWindsor.Extensions
{
    public static class LifetimeExtensions
    {
        public static FluentWindsor WithFluentLifetimeScope(this FluentWindsor fluentWindsor, Func<ILifetimeScope> getScope)
        {
            FluentLifetimeScope.GetCurrentLifetimeScope += getScope;

            FluentLifetimeScope.DisposeLifetimeScope += getScope;

            return fluentWindsor;
        }
    }
}