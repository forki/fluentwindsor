using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor;
using FluentlyWindsor.Lifestyle;

namespace Example.Test.AssemblyC
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ServiceC>().LifestyleCustom<FluentLifetimeManager>());
        }
    }
}