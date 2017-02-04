using System.Web.Mvc;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Example.Test.AssemblyA;
using Example.Test.AssemblyB;
using Example.Test.AssemblyC;
using FluentlyWindsor.Hawkeye;

namespace Example.MVC.Controllers
{
    public interface ITestService
    {
        void DoSomething();
    }

    [Interceptor(typeof(Hawkeye))]
    public class TestService : ITestService
    {
        [Log(LogLevel.Info)]
        public virtual void DoSomething()
        {

        }
    }

    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITestService>().ImplementedBy<TestService>());
        }
    }

    public class DefaultController : Controller
    {
        private readonly ServiceA serviceA;
        private readonly ServiceB serviceB;
        private readonly ServiceC serviceC;
        private readonly ITestService testService;

        public DefaultController(ServiceA serviceA, ServiceB serviceB, ServiceC serviceC, ITestService testService)
        {
            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.serviceC = serviceC;
            this.testService = testService;
        }

        public ViewResult Index()
        {
            testService.DoSomething();

            return View();
        }
    }
}