using System;
using FluentWindsor.Hawkeye.Interfaces;

namespace FluentWindsor.Hawkeye
{
    public class LogAttribute : Attribute
    {
        private readonly LogLevel logLevel;

        public LogAttribute(LogLevel logLevel = LogLevel.Debug)
        {
            this.logLevel = logLevel;
        }

        public LogLevel LogLevel
        {
            get { return logLevel; }
        }

        public ILoggingFormatter Formatter
        {
            get
            {
                return DependencyResolver.Resolve<ILoggingFormatter>();
            }
        }
    }
}