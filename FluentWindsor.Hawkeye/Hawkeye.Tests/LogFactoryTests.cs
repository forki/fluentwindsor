using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using FluentWindsor.Hawkeye;
using FluentWindsor.Hawkeye.Interfaces;
using Hawkeye.Tests.Dummies;
using log4net.Core;
using NUnit.Framework;

namespace Hawkeye.Tests
{
    [TestFixture]
    public class LogFactoryTests
    {
        private WindsorContainer testContainer;

        [SetUp]
        public void SetUp()
        {
            TestLogAppender.Reset();
            testContainer = new WindsorContainer();
            testContainer.Kernel.Resolver.AddSubResolver(new ArrayResolver(testContainer.Kernel));
            testContainer.Install(new WindsorInstaller());
            testContainer.Register(Castle.MicroKernel.Registration.Component.For<TestLoggingTarget>());
        }

        [Test]
        public void Should_Resolve_From_Container()
        {
            Assert.That(testContainer.Resolve<ILogFactory>(), Is.Not.Null);
        }

        [Test]
        public void Should_Be_Able_To_Create_Logger()
        {
            var factory = testContainer.Resolve<ILogFactory>();
            Assert.That(factory.CreateLogger(typeof(LogFactoryTests)), Is.Not.Null);
        }

        [Test]
        public void When_Using_Log_It_Should_Log_To_Appender()
        {
            log4net.Config.XmlConfigurator.Configure();

            var factory = testContainer.Resolve<ILogFactory>();
            var log = factory.CreateLogger<LogFactoryTests>();
            
            log.Info("This is a test");

            Assert.That(TestLogAppender.LastEvent.Level, Is.EqualTo(Level.Info));
            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("This is a test"));
        }
    }
}