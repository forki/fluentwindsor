using Castle.Windsor;
using FluentlyWindsor.EndersJson.Interfaces;
using NUnit.Framework;

namespace FluentlyWindsor.EndersJson.Tests.Windsor
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void Should_resolve_json_client_from_windsor()
        {
            var container = new WindsorContainer();
            container.Install(new EndersJson.Windsor.WindsorInstaller());
            Assert.That(container.Resolve<IJsonService>(), Is.Not.Null);
        }

        [Test]
        public void Should_resolve_sync_json_client_from_windsor()
        {
            var container = new WindsorContainer();
            container.Install(new EndersJson.Windsor.WindsorInstaller());
            Assert.That(container.Resolve<ISyncJsonService>(), Is.Not.Null);
        }
    }
}