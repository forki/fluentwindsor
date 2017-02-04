using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Dependencies;

namespace FluentlyWindsor.WebApi
{
    public class FluentWindsorDependencyResolver : IDependencyResolver
    {
        private IDependencyScope scope;

        public void Dispose()
        {
            scope.Dispose();
            scope = null;
        }

        public object GetService(Type serviceType)
        {
            BeginScope();

            var service = scope.GetService(serviceType);

            if (service == null)
                Debug.WriteLine($"FluentWindsor, WebApi -> Trying to resolve: {serviceType.FullName} but nothing was found");
            else
                Debug.WriteLine($"FluentWindsor, WebApi -> Trying to resolve: {serviceType.FullName} found!");

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            BeginScope();

            var services = scope.GetServices(serviceType);

            if (services == null || !services.Any())
                Debug.WriteLine($"FluentWindsor, WebApi -> Trying to resolve many: {serviceType.FullName} but nothing was found");
            else
                Debug.WriteLine($"FluentWindsor, WebApi -> Trying to resolve many: {serviceType.FullName} found!");


            return services;
        }

        public IDependencyScope BeginScope()
        {
            FluentWindsor.WaitUntilComplete.WaitOne();
            if (scope == null)
                scope = new FluentWindsorDependencyScope();
            return scope;
        }
    }
}