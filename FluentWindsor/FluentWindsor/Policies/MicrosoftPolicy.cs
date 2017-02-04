using System;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public class MicrosoftPolicy : IAssemblyScanningPolicy
    {
        public bool IsAssemblyAllowed(Assembly assembly)
        {
            return !assembly.FullName.StartsWith("Microsoft");
        }

        public bool IsTypeAllowed(Type type)
        {
            return true;
        }
    }
}