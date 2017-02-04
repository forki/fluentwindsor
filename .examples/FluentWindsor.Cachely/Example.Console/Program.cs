using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentlyWindsor;
using FluentlyWindsor.Cachely.Interfaces;

namespace Example.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var TimeoutInMilliseconds = 5000;

            ManualResetEvent wait = new ManualResetEvent(false);

            var container = FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .Create();


            long isComplete = 0;

            var readCounter = 0;
            var createCounter = 0;
            var deleteCounter = 0;

            var cache = container.Resolve<ICache<Guid>>();

            // Reader
            var reader = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    readCounter++;
                    foreach (var p in cache)
                        System.Console.Write(".");
                    Thread.Sleep(1);
                }
            });

            // Creator
            var creator = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    createCounter++;
                    var newGuid = Guid.NewGuid();
                    cache.SetItem(newGuid.ToString("N"), newGuid);
                    System.Console.Write("+");
                    Thread.Sleep(1);
                }
            });

            // Deleter
            var deleter = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    if (cache.Count > 0)
                    {
                        deleteCounter++;
                        var key = cache.AllKeys.First();
                        cache.ExpireItem(key);
                        System.Console.Write("-");
                    }
                    Thread.Sleep(1);
                }
            });

            // Monitor
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeoutInMilliseconds);
                Interlocked.Exchange(ref isComplete, 1);
                wait.Set();
            });

            wait.WaitOne();

            System.Console.WriteLine("Reads: {0}, Creates: {1}, Deletes: {2} in {3} second(s).".FormatWith(readCounter, createCounter, deleteCounter, TimeoutInMilliseconds / 1000));

            System.Console.ReadLine();
        }
    }
}