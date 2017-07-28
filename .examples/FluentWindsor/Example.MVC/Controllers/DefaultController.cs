using System.Web.Mvc;
using Example.Test.AssemblyA;
using Example.Test.AssemblyB;
using Example.Test.AssemblyC;

namespace Example.MVC.Controllers
{
	public class DefaultController : Controller
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

        public ViewResult Index()
        {
            return View();
        }
    }
}