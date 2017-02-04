using System.Web.Http;
using Example.Test.AssemblyA;
using Example.Test.AssemblyB;
using Example.Test.AssemblyC;

namespace Example.MVC.Api
{
    public class DefaultController : ApiController
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;
        private readonly ServiceC serviceC;

        public DefaultController(ServiceA serviceA, ServiceB serviceB, ServiceC serviceC)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello!";
        }
    }
}