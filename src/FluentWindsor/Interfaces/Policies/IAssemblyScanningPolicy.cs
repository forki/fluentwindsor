using System;
using System.Reflection;

namespace FluentlyWindsor.Interfaces.Policies
{
    public interface IAssemblyScanningPolicy
    {
        bool IsAssemblyAllowed(Assembly assembly);
        bool IsTypeAllowed(Type type);
    }
}