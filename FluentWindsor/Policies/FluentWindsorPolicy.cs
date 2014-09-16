using System;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public class FluentWindsorPolicy : IAssemblyScanningPolicy
    {
        public bool IsAssemblyAllowed(Assembly assembly)
        {
            return !assembly.FullName.StartsWith("FluentWindsor");
        }

        public bool IsTypeAllowed(Type type)
        {
            return !type.FullName.StartsWith("FluentWindsor");
        }
    }
}