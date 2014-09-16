using Castle.MicroKernel.Registration;
using FluentlyWindsor.Extensions;
using NUnit.Framework;

namespace FluentWindsor.Tests.Extensions
{
    [TestFixture]
    public class When_We_Register_Types_Using_The_Windsor_RegisterIfNotAlready : Given_We_Are_Extending_Windsor
    {
        [Test]
        public void Then_We_Should_Be_Able_To_Register_Types_Multiple_Times_Without_Exceptions_Being_Raised()
        {
            Container.RegisterIfNotAlready(Component.For<Example.Test.AssemblyA.ServiceA>().LifeStyle.Transient);
            Container.RegisterIfNotAlready(Component.For<Example.Test.AssemblyA.ServiceA>().LifeStyle.Transient);
            Container.RegisterIfNotAlready(Component.For<Example.Test.AssemblyA.ServiceA>().LifeStyle.Transient);
        }
    }
}