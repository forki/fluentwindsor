using System;
using System.Collections.Generic;
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
            return scope.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            BeginScope();
            return scope.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            FluentlyWindsor.FluentWindsor.WaitUntilComplete.WaitOne();
            if (scope == null)
                scope = new FluentWindsorDependencyScope();
            return scope;
        }
    }
}