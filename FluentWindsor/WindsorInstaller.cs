using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor.Interfaces;
using FluentlyWindsor.Interfaces.Policies;
using FluentlyWindsor.Policies;

namespace FluentlyWindsor
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAssemblyScanner>().ImplementedBy<AssemblyScanner>().LifeStyle.Transient.OnlyNewServices());

            container.Register(Component.For<IAssemblyScanningPolicy>().ImplementedBy<CastleWindsorPolicy>().Forward<IAssemblyScanningPolicy>().LifeStyle.Transient.OnlyNewServices());
            container.Register(Component.For<IAssemblyScanningPolicy>().ImplementedBy<MsCorLibPolicy>().Forward<IAssemblyScanningPolicy>().LifeStyle.Transient.OnlyNewServices());
            container.Register(Component.For<IAssemblyScanningPolicy>().ImplementedBy<MicrosoftPolicy>().Forward<IAssemblyScanningPolicy>().LifeStyle.Transient.OnlyNewServices());
            container.Register(Component.For<IAssemblyScanningPolicy>().ImplementedBy<SystemPolicy>().Forward<IAssemblyScanningPolicy>().LifeStyle.Transient.OnlyNewServices());
        }
    }
}