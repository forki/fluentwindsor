using System;
using FluentWindsor.Hawkeye.Interfaces;

namespace FluentWindsor.Hawkeye
{
    public class LogFactory : ILogFactory
    {
        public virtual ILog CreateLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        public virtual ILog CreateLogger<T>()
        {
            var logger = LogManager.GetLogger(typeof (T));
            return logger;
        }
    }
}