using Example.Test.AssemblyA;
using NUnit.Framework;

namespace FluentWindsor.Tests
{
    [TestFixture]
    public class When_Resolving_Services : Given_We_Are_Fluently_Registering
    {
        [Test]
        public void Then_We_Should_Be_Able_To_Resolve_ServiceA()
        {
            Assert.That(Container.Resolve<ServiceA>(), Is.Not.Null);
        }

        [Test]
        public void Then_We_Should_Be_Able_To_Resolve_ServiceB()
        {
            Assert.That(Container.Resolve<ServiceA>(), Is.Not.Null);
        }

        [Test]
        public void Then_We_Should_Be_Able_To_Resolve_ServiceC()
        {
            Assert.That(Container.Resolve<ServiceA>(), Is.Not.Null);
        }
    }
}