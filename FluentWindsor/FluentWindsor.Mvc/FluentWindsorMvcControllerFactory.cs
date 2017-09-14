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
	        var controllerType = FindControllerType(controllerName);
	        return (IController) container.Resolve(controllerType.Name + "_MVC", controllerType);
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
            var results = FluentWindsor.ExecutingAssembly.GetAnyTypeThatIsSubClassOf<System.Web.Mvc.Controller>(AssemblyScanningPolicies.All);
            if (results.Any(x => x.Name.Contains(controllerName)))
				return results.First(x => x.Name.Contains(controllerName));

            throw new MissingControllerException($"The controller '{controllerName}' is nowhere to be found. Have you referenced the assembly? Are you missing a registration or installer somehwere?");
        }
    }

    internal class MissingControllerException : Exception
    {
        public MissingControllerException(string message) : base(message)
        {
        }
    }
}