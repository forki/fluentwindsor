using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FluentlyWindsor.Cachely.Tests.Performance
{
    [TestFixture]
    public class PerformanceTests
    {
        public int TimeoutInMilliseconds = 5000;

        private ManualResetEvent wait;

        [SetUp]
        public void SetUp()
        {
            wait = new ManualResetEvent(false);
        }

        [Test]
        public void Cachely_Cache_Read_Write_Expiry_Test()
        {
            long isComplete = 0;
            var random = new Random();

            int readCounter = 0;
            int createCounter = 0;
            int deleteCounter = 0;

            var cache = new Cache<Guid>();

            // Reader
            var reader = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    readCounter++;
                    foreach (var p in cache)
                        Console.Write(".");
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
                    Console.Write("+");
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
                        cache.ExpireItem((string)key);
                        Console.Write("-");
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

            // Errors
            if (reader.Exception != null)
                Assert.Fail("\r\nNo longer thread safe ->  -> {0}".FormatWith(reader.Exception));

            if (creator.Exception != null)
                Assert.Fail("\r\nNo longer thread safe ->  -> {0}".FormatWith(creator.Exception));

            if (deleter.Exception != null)
                Assert.Fail("\r\nNo longer thread safe ->  -> {0}".FormatWith(deleter.Exception));

            Console.WriteLine("Reads: {0}, Creates: {1}, Deletes: {2} in {3} second(s).".FormatWith(readCounter, createCounter, deleteCounter, TimeoutInMilliseconds / 1000));
        }
    }
}