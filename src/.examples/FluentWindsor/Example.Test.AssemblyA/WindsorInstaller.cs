using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor;
using FluentlyWindsor.Lifestyle;

namespace Example.Test.AssemblyA
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ServiceA>().LifestyleCustom<FluentLifestyleManager>());
        }
    }
}