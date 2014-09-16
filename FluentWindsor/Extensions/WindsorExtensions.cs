using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace FluentlyWindsor.Extensions
{
    public static class WindsorExtensions
    {
        public static void RegisterIfNotAlready<T>(this IWindsorContainer container, params ComponentRegistration<T>[] registrations) where T : class
        {
            foreach (var registration in registrations)
            {
                container.Register(registration.OnlyNewServices());
            }
        }

        [DebuggerStepThrough]
        public static object FaultTolerantResolve(this IWindsorContainer container, Type type)
        {
            try
            {
                return FluentWindsor.ServiceLocator.Resolve(type);
            }
            catch
            {
                return null;
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<object> FaultTolerantResolveAll(this IWindsorContainer container, Type type)
        {
            try
            {
                return FluentWindsor.ServiceLocator.ResolveAll(type).Cast<object>().ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}