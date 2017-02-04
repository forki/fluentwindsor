using Castle.Windsor;
using FluentlyWindsor.Cachely.Interfaces;
using NUnit.Framework;

namespace FluentlyWindsor.Cachely.Tests.Windsor
{
    [TestFixture]
    public class CacheTests
    {
        [Test]
        public void Should_Resolve_String_Cache_From_Container()
        {
            var container = new WindsorContainer();
            container.Install(new FluentlyWindsor.Cachely.Windsor.WindsorInstaller());
            Assert.That(container.Resolve<ICache<string>>(), Is.Not.Null);
        }

        [Test]
        public void Should_Resolve_Double_Cache_From_Container()
        {
            var container = new WindsorContainer();
            container.Install(new FluentlyWindsor.Cachely.Windsor.WindsorInstaller());
            Assert.That(container.Resolve<ICache<double>>(), Is.Not.Null);
        }
    }
}
