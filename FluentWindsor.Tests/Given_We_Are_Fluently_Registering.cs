using System.Reflection;
using Castle.Windsor;
using NUnit.Framework;

namespace FluentWindsor.Tests
{
    public class Given_We_Are_Fluently_Registering
    {
        protected IWindsorContainer Container;

        [SetUp]
        public void SetUp()
        {
            Container = FluentlyWindsor.FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .Create();
        }
    }
}