using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor.Hawkeye.Interfaces;

namespace FluentlyWindsor.Hawkeye
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public static IWindsorContainer Container = null;

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Container = container;

            container.Register(
                Component.For<ILoggingFormatter>()
                    .ImplementedBy<AggregateLoggingFormatter>()
                    .LifeStyle.Transient.IsFallback());

            container.Register(
                Classes.FromAssembly(Assembly.GetExecutingAssembly())
                    .BasedOn<IFormatter>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

            container.Register(
                Component
                    .For<Hawkeye>()
                    .LifeStyle
                    .Singleton);

            container.Register(Component.For<ILogFactory>().ImplementedBy<LogFactory>().LifeStyle.Transient);
        }
    }
}