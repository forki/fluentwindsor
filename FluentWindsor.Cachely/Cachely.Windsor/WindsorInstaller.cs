using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor.Cachely.Interfaces;

namespace FluentlyWindsor.Cachely.Windsor
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof (ICache<>)).ImplementedBy(typeof (Cache<>)).LifeStyle.Transient);
        }
    }
}
