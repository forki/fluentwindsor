using Castle.MicroKernel.Registration;
using Example.Test.AssemblyA;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Interfaces;
using FluentlyWindsor.Policies;
using NUnit.Framework;

namespace FluentWindsor.Tests
{
    [TestFixture]
    public class When_We_Scan_For_Types_Using_Extensions : Given_We_Are_Fluently_Registering
    {
        [Test]
        public void Then_We_Should_Be_Able_To_Discover_Assemblies_That_Has_Any_Type_That_Implements_An_Interface()
        {
            var service = Container.Resolve<IAssemblyScanner>();
            var assemblies = service.FindAssemblies(x => x.HasAnyTypeThatImplementsInterface<IWindsorInstaller>(AssemblyScanningPolicies.All));
            Assert.That(assemblies.Count, Is.EqualTo(4));
        }

        [Test]
        public void Then_We_Should_Be_Able_To_Discover_Assemblies_That_Has_Any_Type_That_Inherits_From()
        {
            var service = Container.Resolve<IAssemblyScanner>();
            var assemblies = service.FindAssemblies(x => x.HasAnyTypeThatIsSubClassOf<BaseObject>(AssemblyScanningPolicies.All));
            Assert.That(assemblies.Count, Is.EqualTo(1));
        }
    }
}