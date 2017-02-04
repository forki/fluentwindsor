using System;

namespace FluentWindsor.Hawkeye
{
    public class LoggingFormatterParams
    {
        public readonly Exception Exception;
        public readonly IInvocation Invocation;
        public readonly TimeSpan ElapsedExecution;

        public LoggingFormatterParams(IInvocation invocation, TimeSpan elapsedExecution, Exception exception)
        {
            Exception = exception;
            Invocation = invocation;
            ElapsedExecution = elapsedExecution;
        }
    }
}