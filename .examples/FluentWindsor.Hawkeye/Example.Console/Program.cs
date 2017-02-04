using System;
using System.Reflection;
using System.Threading;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor;
using FluentlyWindsor.Hawkeye;

namespace Example.Console
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

    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .Create();

            var service = container.Resolve<ITestService>();

            service.DoSomething();
            
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}