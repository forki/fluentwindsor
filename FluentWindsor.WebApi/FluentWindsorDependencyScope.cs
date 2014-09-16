using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.WebApi
{
    public class FluentWindsorDependencyScope : IDependencyScope
    {
        private object service;
        private IEnumerable<object> services;

        public FluentWindsorDependencyScope()
        {
            FluentlyWindsor.FluentWindsor.WaitUntilComplete.WaitOne();
        }

        public object GetService(Type serviceType)
        {
            service = FluentlyWindsor.FluentWindsor.ServiceLocator.FaultTolerantResolve(serviceType);
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            services = FluentlyWindsor.FluentWindsor.ServiceLocator.FaultTolerantResolveAll(serviceType);
            return services;
        }

        public void Dispose()
        {
            if (service != null)
                FluentlyWindsor.FluentWindsor.ServiceLocator.Release(service);
            if (services != null)
                foreach(var s in services)
                    FluentlyWindsor.FluentWindsor.ServiceLocator.Release(s);
        }
    }
}