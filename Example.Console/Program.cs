using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Example.Test.AssemblyC;
using FluentlyWindsor;

namespace Example.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .Create();

            var serviceC = container.Resolve<ServiceC>();

            serviceC.Execute();
        }
    }
}