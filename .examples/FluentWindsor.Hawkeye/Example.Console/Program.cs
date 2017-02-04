using System.Reflection;
using EndersJson;
using EndersJson.Interfaces;
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

            var jsonService = container.Resolve<IJsonService>();

            for (int i = 0; i < 100; i++)
            {
                var result = jsonService.Get<dynamic>("http://localhost:49365/api/default").Result;
                System.Console.WriteLine("Received WebAPI data -> " + result);
            }
        }
    }
}