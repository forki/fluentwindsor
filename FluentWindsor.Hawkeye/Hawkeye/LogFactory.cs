using System;
using FluentlyWindsor.Hawkeye.Interfaces;
using log4net;

namespace FluentlyWindsor.Hawkeye
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