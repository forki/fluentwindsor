using Castle.Windsor;
using FluentWindsor.Hawkeye;
using NUnit.Framework;

namespace Hawkeye.Tests
{
    [TestFixture]
    public class When_resolving_an_interceptor
    {
        private readonly IWindsorContainer testContainer = new WindsorContainer();

        [SetUp]
        public void Context()
        {
            testContainer.Install(new WindsorInstaller());
        }

        [Test]
        public void Then_instance_should_be_not_be_null()
        {
            Assert.That(testContainer.Resolve<FluentWindsor.Hawkeye.Hawkeye>(), Is.Not.Null);
        }
    }
}
