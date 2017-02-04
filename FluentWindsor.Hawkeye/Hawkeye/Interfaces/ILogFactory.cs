using System;

namespace FluentWindsor.Hawkeye.Interfaces
{
    public interface ILogFactory
    {
        ILog CreateLogger(Type type);
        ILog CreateLogger<T>();
    }
}