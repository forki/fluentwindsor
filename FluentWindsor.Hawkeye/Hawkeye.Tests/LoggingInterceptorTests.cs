using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using FluentlyWindsor.Hawkeye.Tests.Dummies;
using log4net.Core;
using NUnit.Framework;

namespace FluentlyWindsor.Hawkeye.Tests
{
	[TestFixture]
    public class LoggingInterceptorTests
    {
        private IWindsorContainer testContainer;

        [SetUp]
        public void Context()
        {
            TestLogAppender.Reset();
            testContainer = new WindsorContainer();
            testContainer.Kernel.Resolver.AddSubResolver(new ArrayResolver(testContainer.Kernel));
            testContainer.Install(new WindsorInstaller());
            testContainer.Register(Castle.MicroKernel.Registration.Component.For<TestLoggingTarget>());
        }

        [Test]
        public void Then_A_Method_Call_Should_Be_Logged()
        {
            var instance = testContainer.Resolve<TestLoggingTarget>();
            instance.DebugMethod();

            TestLogAppender.WaitOne();

            Assert.That(TestLogAppender.LastEvent, Is.Not.Null);
            Assert.That(TestLogAppender.LastEvent.Level, Is.EqualTo(Level.Debug));
            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("DebugMethod"));
        }

        [Test]
        public void Then_A_Method_Call_That_Throws_Should_Be_Logged()
        {
            var instance = testContainer.Resolve<TestLoggingTarget>();
            try {instance.ExceptionMethod();} catch{}

            TestLogAppender.WaitOne();

            Assert.That(TestLogAppender.LastEvent, Is.Not.Null);
            Assert.That(TestLogAppender.LastEvent.Level, Is.EqualTo(Level.Error));
            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("A test error that should be visible to the logging framework ... "));
        }

        [Test]
        public void Then_A_Method_With_Info_Should_Be_Logged_Using_Info_Category()
        {
            var instance = testContainer.Resolve<TestLoggingTarget>();
            instance.InfoMethod();

            TestLogAppender.WaitOne();

            Assert.That(TestLogAppender.LastEvent, Is.Not.Null);
            Assert.That(TestLogAppender.LastEvent.Level, Is.EqualTo(Level.Info));
            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("InfoMethod"));
        }

        [Test]
        public void Then_A_Method_With_Info_Should_Not_Log_Stacktrace()
        {
            var instance = testContainer.Resolve<TestLoggingTarget>();
            instance.InfoMethod();

            TestLogAppender.WaitOne();

            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("Stacktrace"), Is.False);
        }

        [Test]
        public void Then_A_Method_With_Debug_Should_Log_Stacktrace()
        {
            var instance = testContainer.Resolve<TestLoggingTarget>();
            instance.DebugMethod();

            TestLogAppender.WaitOne();

            Assert.That(TestLogAppender.LastEvent.RenderedMessage.Contains("Stacktrace"));
        }
    }
}