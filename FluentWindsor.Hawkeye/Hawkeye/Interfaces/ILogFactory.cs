using System;
using log4net;

namespace FluentlyWindsor.Hawkeye.Interfaces
{
    public interface ILogFactory
    {
        ILog CreateLogger(Type type);
        ILog CreateLogger<T>();
    }
}