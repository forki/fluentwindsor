using System;
using System.Threading;
using log4net.Appender;
using log4net.Core;

namespace FluentlyWindsor.Hawkeye.Tests.Dummies
{
    public class TestLogAppender : AppenderSkeleton
    {
        public static LoggingEvent LastEvent = null;
        public static ManualResetEvent Wait = new ManualResetEvent(false);

        protected override void Append(LoggingEvent loggingEvent)
        {
            LastEvent = loggingEvent;
            Wait.Set();
        }

        public static void Reset()
        {
            Wait = new ManualResetEvent(false);
        }

        public static void WaitOne()
        {
            Wait.WaitOne(TimeSpan.FromSeconds(1));
        }

        override protected bool RequiresLayout
        {
            get { return false; }
        }
    }
}