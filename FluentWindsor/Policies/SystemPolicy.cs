using System;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public class SystemPolicy : IAssemblyScanningPolicy
    {
        public bool IsAssemblyAllowed(Assembly assembly)
        {
            return !assembly.FullName.StartsWith("System");
        }

        public bool IsTypeAllowed(Type type)
        {
            return !type.FullName.StartsWith("System");
        }
    }
}