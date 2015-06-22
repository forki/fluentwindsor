using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Castle.Windsor;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Policies;

namespace FluentlyWindsor.Mvc
{
    public class FluentWindsorMvcControllerFactory : IControllerFactory
    {
        private readonly IWindsorContainer container;

        public FluentWindsorMvcControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            FluentWindsor.WaitUntilComplete.WaitOne();
            return (IController) container.Resolve(controllerName + "_MVC", FindControllerType(controllerName));
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Disabled;
        }

        public void ReleaseController(IController controller)
        {
            container.Release(controller);
        }

        private static Type FindControllerType(string controllerName)
        {

            foreach (var ns in FluentWindsorExtensionsConstants.ControllerNamespaces)
            {
                var controllerFullName = ns + "." + controllerName + "Controller";
                var results = FluentWindsor.ExecutingAssembly.GetAnyTypeWithFullName(AssemblyScanningPolicies.All, controllerFullName);
                if (results.Any()) return results.First();
            }

            throw new MissingControllerException(
                string.Format("The controller '{0}' is nowhere to be found. Have you referenced the assembly? Are you missing a registration or installer somehwere?", 
                controllerName));
        }
    }

    internal class MissingControllerException : Exception
    {
        public MissingControllerException(string message) : base(message)
        {
        }
    }
}