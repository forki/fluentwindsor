using System;
using FluentlyWindsor.Hawkeye.Interfaces;

namespace FluentlyWindsor.Hawkeye
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