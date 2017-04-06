using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using FluentlyWindsor.Extensions;

namespace FluentlyWindsor.WebApi
{
    public class FluentWindsorControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            FluentWindsor.WaitUntilComplete.WaitOne();
            var faultTolerantResolve = (IHttpController)FluentWindsor.ServiceLocator.FaultTolerantResolve(controllerType);
            request.RegisterForDispose(new FluentWindsorControllerDeactivator(() => FluentWindsor.ServiceLocator.Release(faultTolerantResolve)));
            return faultTolerantResolve;
        }
    }

    public class FluentWindsorControllerDeactivator : IDisposable
    {
        private readonly Action release;

        public FluentWindsorControllerDeactivator(Action release)
        {
            this.release = release;
        }

        public void Dispose()
        {
            this.release();
        }
    }
}