using System;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public class CastleWindsorPolicy : IAssemblyScanningPolicy
    {
        public bool IsAssemblyAllowed(Assembly assembly)
        {
            return !assembly.FullName.StartsWith("Castle.Windsor");
        }

        public bool IsTypeAllowed(Type type)
        {
            return !type.FullName.StartsWith("Castle.Windsor");
        }
    }
}